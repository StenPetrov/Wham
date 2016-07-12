using System;
using DotLiquid.Exceptions;
using System.IO;

namespace Wham
{
    public class TemplateFileSystem : DotLiquid.FileSystems.IFileSystem
    {
        public string LocalPath { get; protected set; }

        public Func<string, Stream> FCreateOutputStream { get; set; }

        public class TemplateFileSystemEventArgs : EventArgs
        {
            public bool IsWriteEvent { get; set; }

            public string Name { get; set; }

            public Stream Stream { get; set; }
        }

        public event EventHandler<TemplateFileSystemEventArgs> FileEvent;

        public TemplateFileSystem (string localPath = null, Func<string, Stream> createOutputStream = null)
        {
            LocalPath = localPath;
            FCreateOutputStream = createOutputStream;
        }

        public void NotifyFileWritten (string name, StreamWriter stream)
        {
            if (FileEvent != null) {
                FileEvent (this, new TemplateFileSystemEventArgs { Name = name, Stream = stream.BaseStream, IsWriteEvent = true });
            }
        }

        public Stream CreateOutputStream (string outputName)
        {
            Stream res = null;

            try {
                if (FCreateOutputStream == null) {
                    var dir = Path.GetDirectoryName (outputName);
                    if (!Directory.Exists (dir))
                        Directory.CreateDirectory (dir);

                    res = File.Create (outputName);
                } else
                    res = FCreateOutputStream (outputName);
            } catch (Exception x) {
                throw new WhamException ($"[TFJAKPQOUMN] Error writing to '{outputName}' error: " + x.Message, x);
            }

            return res;
        }

        #region IFileSystem implementation

        public string ReadTemplateFile (DotLiquid.Context context, string templateName)
        {
            try {
                templateName = templateName.Trim ('\'', '"');

                context.GetTracer ().Info ("Loading template: " + templateName);

                var res = BuiltInTemplates.GetResourceTemplate (templateName);

                if (string.IsNullOrEmpty (res)) {
                    if (!string.IsNullOrEmpty (LocalPath)) {
                        templateName = System.IO.Path.Combine (LocalPath, templateName);

                        if (System.IO.File.Exists (templateName))
                            res = System.IO.File.ReadAllText (templateName);
                        else
                            throw new WhamTemplateException ("[FKASIHQJWKTP] Template not found: " + templateName);
                    } else {
                        throw new WhamTemplateException ($"[FHBAOUTOPQA] Template not loaded: '{templateName}'");
                    }
                }

                return res;
            } catch (Exception x) {
                throw new WhamTemplateException ($"[FKJJAHQROIZ] Template error accessing '{templateName}': {x.Message}", x);
            }
        }

        #endregion
    }
}

