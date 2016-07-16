using DotLiquid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wham
{
    public static class TextFilters
    {
        private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public static string CamelCase(object input, Context context = null)
        {
            string result = null;
            if (input != null)
            {
                result = PascalCase(input, context);
                if (result != null && result.Length > 0)
                {
                    result = "" + char.ToLowerInvariant(result[0]) + (result.Length > 1 ? result.Substring(1) : "");
                }
            }
            return result;
        }

        public static string PascalCase(object input, Context context = null)
        {
            string result = null;
            if (input != null)
            {
                result = input.ToString();
                if (result != null && result.Length > 0)
                {
                    if (result.IndexOf(' ') > 0)
                        result = textInfo.ToTitleCase(result);
                    else
                        result = "" + char.ToUpperInvariant(result[0]) + (result.Length > 1 ? result.Substring(1) : "");
                }
            }

            return result;
        }

        public static string Braces(object input, Context context = null)
        {
            return "{" + input + "}";
        }
    }
}
