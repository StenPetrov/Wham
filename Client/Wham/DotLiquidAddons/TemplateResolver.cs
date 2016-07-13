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

        public static string CS_ClassTemplate {
            get {
                return cs_ClassTemplate = cs_ClassTemplate ?? GetTemplateContents ("CS_ClassTemplate.dlq");
            }
        }

        private static string whamMasterTemplate = null;

        public static string WhamMasterTemplate {
            get {
                return whamMasterTemplate = whamMasterTemplate ?? GetTemplateContents ("WhamMasterTemplate.dlq");
            }
        }

        private static readonly Dictionary<string, Func<string, string>> RegisteredTemplates =
            new Dictionary<string, Func<string, string>> () {
            // the default resolver by default uses built-in templates
            {DefaultTemplateResolverKey, (templateName)=>{
                    var asm = Assembly.GetExecutingAssembly ();
                    string fullResName = asm.GetManifestResourceNames ().FirstOrDefault (rn => rn.EndsWith (templateName));

                    if (fullResName != null) {
                        using (var sr = new StreamReader (asm.GetManifestResourceStream (fullResName))) {
                            return sr.ReadToEnd ();
                        }
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
        public static void RegisterTemplate (string name, Func<string, string> getTemplateContentsByName)
        {
            RegisteredTemplates [name.ToLowerInvariant ()] = getTemplateContentsByName;
        }

        public static string GetTemplateContents(string name)
        {
            if (name != null) {
                Func<string, string> getTemplateContentsByName = null;
                if (RegisteredTemplates.ContainsKey (name.ToLowerInvariant ()))
                    getTemplateContentsByName = RegisteredTemplates [name.ToLowerInvariant ()];

                string contents = getTemplateContentsByName?.Invoke (name);

                if (contents == null) {
                    if (RegisteredTemplates.ContainsKey (DefaultTemplateResolverKey))
                        getTemplateContentsByName = RegisteredTemplates [DefaultTemplateResolverKey];

                    contents = getTemplateContentsByName?.Invoke (name);
                }

                return contents;
            }

            throw new WhamTemplateException ("[BHAIQHHARNS] Template name is required to resolve a template");
        }
    }
}

