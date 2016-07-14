using System;
using DotLiquid;
using System.IO;
using System.Collections.Generic;
using DotLiquid.Exceptions;
using System.Reflection;

namespace Wham
{
    public class FileTag : Block
    {

        string OutputFile { get; set; }

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {
            if (!string.IsNullOrEmpty(markup))
            {
                OutputFile = markup.Trim();
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var hash = context[FolderTag.FileAndFolderOutputHash] as Hash;

            var tracer = context.GetTracer();

            string outFolder = null;

            object parentFolder = null;

            if (hash != null && hash.TryGetValue(FolderTag.CurrentFolderVariableName, out parentFolder))
                outFolder = "" + parentFolder;

            var outFile = context[OutputFile] as string ?? OutputFile;

            string outputFullFileName = string.IsNullOrEmpty(outFolder) ? outFile : Path.Combine(outFolder, outFile);

            context.Stack(() =>
            {
                context[FolderTag.FileAndFolderOutputHash] = FolderTag.CreateCurrentOutputHash(outFolder, null, null, outputFullFileName);

                if (Template.FileSystem is ITemplateFileSystem)
                {
                    var stream = ((ITemplateFileSystem)Template.FileSystem).CreateOutputStream(outputFullFileName);

                    tracer.Info("[FSIOAUHSJFQ] Outputting to: " + outputFullFileName + " req: " + outputFullFileName);
                    using (var outputTo = new StreamWriter(stream))
                    {
                        RenderAll(NodeList, context, outputTo);

                        outputTo.Flush();
                        ((ITemplateFileSystem)Template.FileSystem).NotifyFileWritten(outputFullFileName, outputTo);
                    }
                }
                else {
                    throw new WhamException("[FTHJAOUQZZMQ] File tag requires ITemplateFileSystem");
                }
            });
        }

    }
}