using System;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Wham
{
    public static class BuiltInTemplates
    {
        private static string cs_ClassTemplate = null;

        public static  string CS_ClassTemplate
        {
            get
            {
                return cs_ClassTemplate = cs_ClassTemplate ?? GetResourceTemplate("CS_ClassTemplate.dlq");
            }
        }

        public static string GetResourceTemplate(string name)
        {
            var asm = Assembly.GetExecutingAssembly(); 
            string fullResName = asm.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(name));

            if (fullResName != null)
            {
                using (var sr = new StreamReader(asm.GetManifestResourceStream(fullResName)))
                {
                    return sr.ReadToEnd();
                }
            }

            return null;
        }
    }
}

