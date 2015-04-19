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
            var hash = context["output"] as Hash;

            string outFolder = null;

            object parentFolder = null; 

            if (hash != null && hash.TryGetValue("folder", out parentFolder))
                outFolder = "" + parentFolder;

            var outFile = context[OutputFile] as string ?? OutputFile;
             
            string outputFullFileName = string.IsNullOrEmpty(outFolder) ? outFile : Path.Combine(outFolder, outFile);
                
            context.Stack(() =>
                {     
                    context["output"] = Hash.FromAnonymousObject(new
                        {
                            folder = outFolder,
                            parentFolder = "" + parentFolder, 
                            file = outFile,
                            fullFileName = outputFullFileName
                        });
                     
                    if (Template.FileSystem is TemplateFileSystem)
                    {
                        var str = ((TemplateFileSystem)Template.FileSystem).CreateOutputStream(outputFullFileName);
                        
                        using (var outputTo = new StreamWriter(str))
                        {  
                            RenderAll(NodeList, context, outputTo);  

                            outputTo.Flush();
                            ((TemplateFileSystem)Template.FileSystem).NotifyFileWritten(outputFullFileName, outputTo);
                        }
                    }
                    else
                        RenderAll(NodeList, context, result); 
                });
        }
         
    }
}