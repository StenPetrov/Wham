using System;
using DotLiquid.Exceptions;
using System.IO;

namespace Wham
{
    public class TemplateFileSystem : DotLiquid.FileSystems.IFileSystem
    {
        public string LocalPath { get; protected set; }

        public Func<string,Stream> FCreateOutputStream { get; set; }

        public class TemplateFileSystemEventArgs: EventArgs
        {
            public bool IsWriteEvent { get; set; }

            public string Name{ get; set; }

            public Stream Stream { get; set; }
        }

        public event EventHandler<TemplateFileSystemEventArgs> FileEvent;

        public TemplateFileSystem(string localPath = null, Func<string,Stream> createOutputStream = null)
        {
            LocalPath = localPath; 
            FCreateOutputStream = createOutputStream;
        }

        public void NotifyFileWritten(string name, StreamWriter stream)
        {
            if (FileEvent != null)
            {
                FileEvent(this, new TemplateFileSystemEventArgs{ Name = name, Stream = stream.BaseStream, IsWriteEvent = true });
            }
        }

        public Stream CreateOutputStream(string outputName)
        {
            Stream res = null;

            if (FCreateOutputStream == null)
            {
                var dir = Path.GetDirectoryName(outputName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                
                res = File.Create(outputName);
            }
            else
                res = FCreateOutputStream(outputName);

            return res;
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

