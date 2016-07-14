using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Ionic.Zip;
using Newtonsoft.Json;
using WhamOnline.Models;

namespace WhamOnline.Controllers
{
    public class WhamGeneratorController : ApiController
    {
        // GET: api/WhamGenerator
        public IEnumerable<string> Get()
        {
            return Directory.GetDirectories(GetDataPath())
                .Select(dir => Path.GetFileName(dir));
        }

        // GET: api/WhamGenerator/5
        public HttpResponseMessage Get(Guid id)
        {
            string taskResult = CreateZipFile(id);

            if (File.Exists(taskResult))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var stream = new FileStream(taskResult, FileMode.Open, FileAccess.Read);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = Path.GetFileName(taskResult),
                    };
                return result;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        // POST: api/WhamGenerator
        // returns { "taskId": "guid-123-guid-123", "errors": "something error or null if no errors"}
        [HttpPost]
        public async Task<HttpResponseMessage> PostJsonSchema([FromBody] AppGenConfig appGenConfig)
        {
            Guid taskId = Guid.NewGuid();
            string errors = null;

            try
            {
                ValidateAppConfig(appGenConfig);

                string taskFolder = GetDataPath(taskId.ToString());
                Directory.CreateDirectory(taskFolder);

                string taskRequestFile = GetDataPath(taskId.ToString(), taskId.ToString() + ".request");
                File.WriteAllText(taskRequestFile, JsonConvert.SerializeObject(appGenConfig, Formatting.Indented));

                var engine = new Wham.WhamEngine(taskFolder);
                engine.Context["appGen"] = appGenConfig;
                errors = engine.Liquidize(appGenConfig.AppOptions.Theme + "Theme.dlq");

                errors = errors?.Replace(taskFolder, string.Empty)?.Trim();

                if (string.IsNullOrEmpty(errors)) // pre-create the zip if there were no errors
                    CreateZipFile(taskId);
            }
            catch (Exception x)
            {
                errors = x.ToString();
            }

            if (!string.IsNullOrEmpty(errors))
            {
                string taskErrorFile = GetDataPath(taskId.ToString(), taskId.ToString() + ".error.log");
                File.WriteAllText(taskErrorFile, errors);
            }

            var response = Request.CreateResponse(
                string.IsNullOrEmpty(errors) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                new
                {
                    taskId,
                    errors,
                });

            return response;
        }

        // DELETE: api/WhamGenerator/5
        public void Delete(Guid id)
        {
            string delTaskFolder = GetDataPath(id.ToString());
            if (Directory.Exists(delTaskFolder))
            {
                Directory.Delete(delTaskFolder, true);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private void ValidateAppConfig(AppGenConfig appGenConfig)
        {
            if (appGenConfig == null) ThrowValidationError("App config required");
            if (string.IsNullOrWhiteSpace(appGenConfig.AppOptions.AppName))
                ThrowValidationError("App name required");
            if (appGenConfig.AppOptions.AppName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                ThrowValidationError("App name must be a valid file name");
        }

        private void ThrowValidationError(string errorContent)
        {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    error = errorContent,
                }))
            });
        }

        private string GetDataPath(string taskFolderName = null, string fileName = null)
        {
            string tasksFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WhamOutput");

            if (!Directory.Exists(tasksFolderPath))
            {
                // creating the folder for all tasks
                Directory.CreateDirectory(tasksFolderPath);
            }

            if (taskFolderName != null)
            {
                tasksFolderPath = Path.Combine(tasksFolderPath, taskFolderName);
                if (!Directory.Exists(tasksFolderPath))
                {
                    Directory.CreateDirectory(tasksFolderPath);
                    TaskFolderCreated(tasksFolderPath);
                }

                if (fileName != null)
                {
                    // returning a file name
                    tasksFolderPath = Path.Combine(tasksFolderPath, fileName);
                }
            }

            return tasksFolderPath;
        }

        private string CreateZipFile(Guid taskId, bool recreate = false)
        {
            string taskFolder = GetDataPath(taskId.ToString());
            string taskResultFile = GetDataPath(taskId.ToString(), taskId.ToString() + ".zip");

            if (File.Exists(taskResultFile))
            {
                if (recreate)
                    File.Delete(taskResultFile);
                else
                    return taskResultFile;
            }

            using (ZipFile zip = new ZipFile(taskResultFile))
            {
                zip.AddDirectory(taskFolder);
                zip.Save();
            }

            TaskZipFileCreated(taskResultFile);

            return taskResultFile;
        }

        protected virtual void TaskZipFileCreated(string zipFileName)
        {
            Console.WriteLine(zipFileName);
        }

        protected virtual void TaskFolderCreated(string taskFolder)
        {
        }
    }
}
