using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace Wham
{
    public static class TemplateResolver
    {
        public const string DefaultTemplateResolverKey = "*";

        private static string cs_ClassTemplate = null;

        public static string CS_ClassTemplate
        {
            get
            {
                return cs_ClassTemplate = cs_ClassTemplate ?? GetTemplateContents("CS_ClassTemplate.dlq");
            }
        }

        private static string whamMasterTemplate = null;

        public static string WhamMasterTemplate
        {
            get
            {
                return whamMasterTemplate = whamMasterTemplate ?? GetTemplateContents("WhamMasterTemplate.dlq");
            }
        }

        private static readonly Dictionary<string, Func<string, Stream>> RegisteredTemplates =
            new Dictionary<string, Func<string, Stream>>() {
            // the default resolver by default uses built-in templates
            {DefaultTemplateResolverKey, (templateName)=>{
                    var asm = Assembly.GetExecutingAssembly ();
                    string fullResName = asm.GetManifestResourceNames ().FirstOrDefault (rn => rn.EndsWith (templateName));

                    if (fullResName != null) {
                        return  asm.GetManifestResourceStream (fullResName);
                    }

                    return null;
                }}
        };

        /// <summary>
        /// Registers a template resolver, if the name is "*" the resolver is used for all templates not found by name.
        /// Only one resolver per name, including *-resolver
        /// </summary>
        /// <param name="name">Template name. "*" for default template resolver. Can't be null.</param>
        /// <param name="getTemplateContentsByName">Returns template contents by name or null if not found.</param>
        public static void RegisterTemplate(string name, Func<string, Stream> getTemplateContentsByName)
        {
            RegisteredTemplates[name.ToLowerInvariant()] = getTemplateContentsByName;
        }

        public static string GetTemplateContents(string name)
        {
            Stream contentsStream = GetTemplateStream(name);

            if (contentsStream != null)
            {
                var reader = new StreamReader(contentsStream);
                var contents = reader.ReadToEnd();
                return contents;
            }

            throw new WhamTemplateException("[BHAIQHHARNS] Template name is required to resolve a template");
        }

        public static Stream GetTemplateStream(string name)
        {
            Stream contentsStream = null;

            if (name != null)
            {
                Func<string, Stream> getTemplateContentsByName = null;
                if (RegisteredTemplates.ContainsKey(name.ToLowerInvariant()))
                    getTemplateContentsByName = RegisteredTemplates[name.ToLowerInvariant()];

                contentsStream = getTemplateContentsByName?.Invoke(name);

                if (contentsStream == null)
                {
                    if (RegisteredTemplates.ContainsKey(DefaultTemplateResolverKey))
                        getTemplateContentsByName = RegisteredTemplates[DefaultTemplateResolverKey];

                    contentsStream = getTemplateContentsByName?.Invoke(name);
                }
            }

            return contentsStream;
        }
    }
}

