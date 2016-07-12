using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WhamOnline.Controllers
{
    public class WhamGeneratorController : ApiController
    {
        // GET: api/WhamGenerator
        public IEnumerable<string> Get()
        {
            return Directory.GetDirectories(GetDataPath());
        }

        // GET: api/WhamGenerator/5
        public HttpResponseMessage Get(Guid id)
        {
            var path = GetDataPath(id.ToString(), id.ToString() + ".zip");
            if (File.Exists(path))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        // POST: api/WhamGenerator
        [HttpPost]
        public async Task<HttpResponseMessage> PostJsonSchema()
        {
            string request = await Request.Content.ReadAsStringAsync();

            Guid taskId = Guid.NewGuid();
            string taskFolder = GetDataPath(taskId.ToString());
            Directory.CreateDirectory(taskFolder);

            string taskRequestFile = GetDataPath(taskId.ToString(), taskId.ToString() + ".request");
            File.WriteAllText(taskRequestFile, request);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(taskId.ToString())
            };
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

        private string GetDataPath(string taskFolderName = null, string fileName = null)
        {
            string tasksFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists(tasksFolderPath))
            {
                Directory.CreateDirectory(tasksFolderPath);
            }

            if (taskFolderName != null)
            {
                if (fileName != null)
                    return Path.Combine(tasksFolderPath, taskFolderName, fileName);

                return Path.Combine(tasksFolderPath, taskFolderName);
            }

            return tasksFolderPath;
        }
    }
}
