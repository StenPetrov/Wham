using System.Web;
using System.Web.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace WhamOnline
{
    public class Global : HttpApplication
    {
        private static string s_templatesPath;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // remove the XML formatter, json only supported
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // init Wham to work on the server
            InitWham(Server.MapPath(@"~/App_Data/Templates"));
        }

        public static void InitWham(string templatesFolderPath)
        {
            s_templatesPath = templatesFolderPath;
            InitWhamTemplateResolver();
            Wham.WhamEngine.InitEngine(new ServerFileSystem());
        }

        internal static IEnumerable<string> InitWhamTemplateResolver()
        {
            var templateFiles = System.IO.Directory.GetFiles(s_templatesPath, "*.*", System.IO.SearchOption.AllDirectories)
                                      .ToDictionary(t => System.IO.Path.GetFileName(t).ToLowerInvariant());

            var hiddenFiles = templateFiles.Keys.Where(f => f.StartsWith(".")).ToList();
            hiddenFiles.ForEach(f => templateFiles.Remove(f));

            Wham.TemplateResolver.RegisterTemplate(
                Wham.TemplateResolver.DefaultTemplateResolverKey,
                (templateName) =>
                {
                    if (templateFiles.ContainsKey(templateName.ToLowerInvariant()))
                    {
                        string templateFile = templateFiles[templateName.ToLowerInvariant()];
                        return System.IO.File.ReadAllText(templateFile);
                    }
                    return null;
                });

            return templateFiles.Keys;
        }
    }
}
