using System;
using DotLiquid;
using System.IO;
using System.Text;

namespace Wham
{
    public class FolderTag : Block
    {
        public const string FileAndFolderOutputHash = "output";

        public const string CurrentFolderVariableName = "currentFolder";
        public const string LastFolderVariableName = "lastFolder";
        public const string ParentFolderVariableName = "parentFolder";
        public const string OutputFullFilePathVariableName = "fullFileName";
        public const string OutputFileVariableName = "fileName";

        string OutputFolder { get; set; }

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                OutputFolder = markup.Trim();
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var hash = context[FileAndFolderOutputHash] as Hash;

            var outFolder = context[OutputFolder] as string ?? OutputFolder;

            object parentFolder = null;

            if (hash == null || !hash.TryGetValue(FolderTag.CurrentFolderVariableName, out parentFolder))
                parentFolder = Directory.GetCurrentDirectory();

            var outFullFolder = Path.Combine("" + parentFolder, outFolder);

            context.Stack(() =>
            {
                context[FileAndFolderOutputHash] = CreateCurrentOutputHash(outFullFolder,
                                                                             outFolder,
                                                                             parentFolder?.ToString(),
                                                                             null);
                try
                {
                    if (!Directory.Exists(outFullFolder))
                        Directory.CreateDirectory(outFullFolder);
                }
                catch (Exception x)
                {
                    throw new WhamException("[FTHAKHNZNBVFR] Folder error: " + outFolder, x);
                }

                StringBuilder sb = new StringBuilder();
                using (StringWriter swt = new StringWriter(sb))
                {
                    RenderAll(NodeList, context, swt);
                    result.Write(sb.ToString().Trim());
                }
            });
        }

        public static Hash CreateCurrentOutputHash(string outFullFolder, string lastFolder, string parentFolder,
                                                    string outFullFileName = null)
        {
            var hash = Hash.FromAnonymousObject(new { });

            if (outFullFolder != null)
            {
                hash.Add(CurrentFolderVariableName, outFullFolder);
                hash.Add(ParentFolderVariableName, parentFolder
                   ?? (outFullFolder != null ? Directory.GetParent(outFullFolder).FullName : null));
            }

            if (lastFolder != null)
                hash.Add(LastFolderVariableName, lastFolder);

            if (outFullFileName != null)
            {
                hash.Add(OutputFullFilePathVariableName, outFullFileName);
                hash.Add(OutputFileVariableName, Path.GetFileName(outFullFileName));
            }

            return hash;
        }
    }
}