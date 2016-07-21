using System;
using DotLiquid;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Wham
{
    public class UnzipTag : Block
    {
        public const string FileAndFolderOutputHash = "output";

        public const string CurrentFolderVariableName = "currentFolder";
        public const string LastFolderVariableName = "lastFolder";
        public const string ParentFolderVariableName = "parentFolder";
        public const string OutputFullFilePathVariableName = "fullFileName";
        public const string OutputFileVariableName = "fileName";

        string ZipFileName { get; set; }

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                ZipFileName = markup.Trim();
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var hash = context[FileAndFolderOutputHash] as Hash;

            try
            {
                object parentFolder = null;

                if (hash == null || !hash.TryGetValue(FolderTag.CurrentFolderVariableName, out parentFolder))
                    parentFolder = Directory.GetCurrentDirectory();

                string unpackDirectory = parentFolder.ToString();

                Stream zipStream;

                if (!Path.IsPathRooted(ZipFileName))
                {
                    zipStream = TemplateResolver.GetTemplateStream(ZipFileName);
                }
                else
                {
                    zipStream = File.OpenRead(ZipFileName);
                }

                using (ZipFile zip = ZipFile.Read(zipStream))
                {
                    foreach (ZipEntry file in zip)
                    {
                        file.Extract(unpackDirectory, ExtractExistingFileAction.Throw);
                    }
                }
            }
            catch (Exception x)
            {
                throw new WhamException("[JFHBNMQWOPTQW] Unzip error: " + ZipFileName, x);
            }
        }
    }
}