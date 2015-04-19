using System;
using DotLiquid;
using System.IO;

namespace Wham
{
    public class FolderTag : Block
    {
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
            var hash = context["output"] as Hash;

            var outFolder = context[OutputFolder] as string ?? OutputFolder;

            object parentFolder = null; 

            if (hash == null || !hash.TryGetValue("folder", out parentFolder))
                parentFolder = Directory.GetCurrentDirectory();
            
            var outFullFolder = Path.Combine("" + parentFolder, outFolder); 

            context.Stack(() =>
                {   
                    context["output"] = Hash.FromAnonymousObject(new
                            {
                                folder = outFullFolder,
                                lastFolder = outFolder,
                                parentFolder = "" + parentFolder, 
                            }); 
                    
                    RenderAll(NodeList, context, result); 
                });
        }
    }
}