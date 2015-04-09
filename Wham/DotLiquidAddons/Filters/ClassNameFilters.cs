using System;
using System.Text.RegularExpressions;
using DotLiquid;
using Newtonsoft.Json.Schema;
using System.Linq;

namespace Wham
{
    public static class ClassNameFilters
    {
        public static WeakReference<Action<string, string, object>> RegisterClassCallback = null;

        public static Regex IsValidTypeName = new Regex("^([a-zA-Z_][a-zA-Z_0-9]*){1}(\\.([a-zA-Z_][a-zA-Z_0-9]*))*$");

        private static JSchema GetSchema(object input, out Context context)
        {
            context = null;

            JSchema schema;
            if (input is IValueTypeConvertible)
            {
                var valueObject = ((IValueTypeConvertible)input).ConvertToValueType();
                 
                schema = valueObject as JSchema;
                if (schema == null && valueObject is JSchemaDrop)
                {
                    schema = ((JSchemaDrop)valueObject).Schema;
                    context = ((JSchemaDrop)valueObject).Context;
                }
            }
            else if (input is JSchemaDrop)
            {
                context = ((JSchemaDrop)input).Context;
                return ((JSchemaDrop)input).Schema; 
            }
            else
                schema = input as JSchema;
            
            return schema;
        }

        // the string constants below come from the .csproj file definition
        public static string RegisterClass(string input, object schema)
        {
            return RegisterProjectItem(input, "Compile", schema);
        }

        public static string RegisterResource(string input, string name)
        {
            return RegisterProjectItem(input, "EmbeddedResource", name);
        }

        public static string RegisterProjectItem(string input, string inputType, object schema)
        {
            Action<string, string, object> callback = null;

            if (RegisterClassCallback != null && RegisterClassCallback.TryGetTarget(out callback))
            {
                callback(input, inputType, schema);
            }
            else
            {
                Console.WriteLine("[FASIDFJHKAJK] Class not registered: " + input);
            }

            return input;
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

        public static string FullClassName(object input, string propName, Context context = null)
        {

            propName = StandardFilters.Capitalize(propName);

            if (input is string)
                return Namespace("" + input) + "." + ClassName("" + input);
            else
            {
                Context ctx;
                var schema = GetSchema(input, out ctx);
                if (schema != null)
                {
                    context = context ?? ctx;

                    if (schema.IsAtomicType())
                        return schema.GetSchemaClrType();
                    else if (schema.Enum != null)
                        return ClassEnums.AddEnum(context, propName, schema);
                    else
                        return "--not a supported type: " + schema;
                } 
            } 
             
            return "--FullClassName(object input), unknown input type: " + input.GetType().Name;
        }

        public static string BaseClassFullName(object oSchema)
        {
            Context context;
            var schema = GetSchema(oSchema, out context);
            if (schema != null)
            {
                var baseSchema = schema.GetBaseSchema();
                if (baseSchema != null)
                    return FullClassName(baseSchema.Title, "UNEXPECTED", context);
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

