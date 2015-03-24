using System;
using System.Text.RegularExpressions;
using DotLiquid;
using Newtonsoft.Json.Schema;

namespace Wham
{
    public static class ClassNameFilters
    {
        public static Regex IsValidTypeName = new Regex("^([a-zA-Z_][a-zA-Z_0-9]*){1}(\\.([a-zA-Z_][a-zA-Z_0-9]*))*$");

        private static JSchema GetSchema(object input)
        {
            JSchema schema;
            if (input is IValueTypeConvertible)
                schema = ((IValueTypeConvertible)input).ConvertToValueType() as JSchema;
            else
                schema = input as JSchema;
            
            return schema;
        }

        public static string Namespace(string input)
        {
            if (IsValidTypeName.IsMatch("" + input))
            { 
                return "Wham" +
                (input.IndexOf('.') > 0 ? "." + input.Substring(0, input.LastIndexOf('.')) : "");
            }
            else
                return null;
        }

        public static string ClassName(string input)
        {
            if (IsValidTypeName.IsMatch("" + input))
            { 
                return input.IndexOf('.') > 0 ?
                    input.Substring(input.LastIndexOf('.') + 1)
                        : input;
            }
            else
                return null;
        }

        public static string FullClassName(object input)
        {
            if (input is string)
                return Namespace("" + input) + "." + ClassName("" + input);
            else
            {
                var schema = GetSchema(input);
                if (schema != null)
                {
                    if (schema.IsAtomicType())
                    {
                        return schema.GetSchemaClrType();
                    }
                }
            } 

            return "--FullClassName(object input), unknown input type: " + input.GetType().Name;
        }

        public static string BaseClassFullName(object oSchema)
        {
            var schema = GetSchema(oSchema);
            if (schema != null)
            {
                var baseSchema = schema.GetBaseSchema();
                if (baseSchema != null)
                    return  FullClassName(baseSchema.Title);
            } 

            return "";
        }

        public static string PreppendIfNotEmpty(string input, string what)
        {
            if (!string.IsNullOrWhiteSpace("" + input))
                return what + input;
            else
                return "" + input; 
        }
    }
}

