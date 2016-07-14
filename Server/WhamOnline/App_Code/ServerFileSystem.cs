using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Wham;

namespace WhamOnline
{
    public class ServerFileSystem : Wham.ITemplateFileSystem
    {
        public Stream CreateOutputStream(string outputFullFileName)
        {
            string folder = Path.GetDirectoryName(outputFullFileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return File.Create(outputFullFileName, 64000);
        }

        public void NotifyFileWritten(string outputFullFileName, StreamWriter outputTo)
        {
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            try
            {
                templateName = templateName.Trim('\'', '"');

                context.GetTracer().Info("Loading template: " + templateName);

                return TemplateResolver.GetTemplateContents(templateName);
            }
            catch (WhamException)
            {
                throw;
            }
            catch (Exception x)
            {
                throw new WhamTemplateException("Error while loading template: " + templateName, x);
            }
        }
    }
}