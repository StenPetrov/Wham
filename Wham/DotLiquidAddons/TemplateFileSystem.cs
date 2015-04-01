using System;
using DotLiquid.Exceptions;

namespace Wham
{
    public class TemplateFileSystem : DotLiquid.FileSystems.IFileSystem
    {
        public string LocalPath { get; protected set; }

        public TemplateFileSystem(string localPath = null)
        {
            LocalPath = localPath;
        }

        #region IFileSystem implementation

        public string ReadTemplateFile(DotLiquid.Context context, string templateName)
        {
            templateName = templateName.Trim('\'', '"');

            var res = BuiltInTemplates.GetResourceTemplate(templateName);

            if (string.IsNullOrEmpty(res) && !string.IsNullOrEmpty(LocalPath))
            {
                templateName = System.IO.Path.Combine(LocalPath, templateName);

                if (System.IO.File.Exists(templateName))
                    res = System.IO.File.ReadAllText(templateName);
            }

            if (string.IsNullOrEmpty(res))
                throw new FileSystemException("[FKASIHQJWKTP] Template not found: " + templateName);
            else
                return res;
        }

        #endregion
    }
}

