using System;
using System.Linq;
using DotLiquid;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;

namespace Wham
{
    public class ClassEnumsTag : Block
    {
        public const string CLASS_ENUMS = "ClassEnums";

        private static Dictionary<string, JSchemaDrop> GetSetEnums(Hash scope)
        {
            Dictionary<string, JSchemaDrop> cen = scope[CLASS_ENUMS] as Dictionary<string, JSchemaDrop>;
            if (cen == null)
            {
                cen = new Dictionary<string, JSchemaDrop>();
                scope[CLASS_ENUMS] = cen;
            }
            return cen;
        }

        public static string AddEnum(Context context, string propName, JSchema schema)
        {
            if (context != null)
            {
                propName += "sEnum";

                var cen = GetSetEnums(context.Scopes.Last());
                 
                if (!cen.ContainsKey(propName))
                    cen.Add(propName, new JSchemaDrop(schema));
                else if (!cen.Values.Any(v => v.Schema == schema))
                {
                    int c = cen.Keys.Where(k => k.StartsWith(propName)).Count() + 1;
                    propName = propName + "_" + c;
                    cen.Add(propName, new JSchemaDrop(schema)); 
                }

                if (schema.Enum.Any(e => e.ToString() == "null"))
                    propName += "?";

                return propName;
            }
            else
                throw new WhamException("[IGUHDAJFKQWET] Missing context");
        }

        public override void Initialize(string tagName, string markup, System.Collections.Generic.List<string> tokens)
        {  
            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, System.IO.TextWriter result)
        { 
            Dictionary<string, JSchemaDrop> cen = context.Scopes.Last()[CLASS_ENUMS] as Dictionary<string, JSchemaDrop>; 

            if (cen != null)
                result.WriteLine("\r\n //ENUMS : " + string.Join(", ", cen.Keys));

            base.Render(context, result);

        }
    }
}

