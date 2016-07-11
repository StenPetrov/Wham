using System;
using Newtonsoft.Json.Schema;
using DotLiquid;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Wham
{
    public class JSchemaDrop : Drop
    {
        private static string[] JSchemaPropertyNames = null;
        private static PropertyInfo[] JSchemaProperties = null;

        public readonly JSchema Schema;

        #region Wrapped JSchema

        protected JSchemaDrop ToDrop(JSchema s)
        {
            return new JSchemaDrop(s);
        }

        protected List<JSchemaDrop> ToDrop(IEnumerable<JSchema> sList)
        {
            List<JSchemaDrop> dList = new List<JSchemaDrop>();

            if (sList != null)
            { 
                foreach (var s in sList)
                    dList.Add(new JSchemaDrop(s)); 
            } 

            return dList;
        }

        protected Dictionary<string, JSchemaDrop> ToDrop(IDictionary<string, JSchema> sDict)
        { 
            var dropDict = new Dictionary<string, JSchemaDrop>();

            if (sDict != null)
            {
                foreach (var pr in sDict)
                    dropDict.Add(pr.Key, new JSchemaDrop(pr.Value)); 
            }

            return dropDict;
        }

        public Uri Id { get { return Schema.Id; } set { Schema.Id = value; } }

        public JSchemaType? Type { get { return Schema.Type; } set { Schema.Type = value; } }

        public string Default { get { return Schema.Default != null ? "" + Schema.Default.Value<string>() : null; } }

        private Dictionary<string, JSchemaDrop> _properties = null;

        public  Dictionary<string, JSchemaDrop> Properties
        { 
            get
            {  
                return (_properties = _properties ?? ToDrop(Schema.Properties));
            }  
        }

        private List<JSchemaDrop> _items = null;

        public List<JSchemaDrop> Items
        { 
            get { return (_items = _items ?? ToDrop(Schema.Items)); }  
        }

        public bool ItemsPositionValidation { get { return Schema.ItemsPositionValidation; } set { Schema.ItemsPositionValidation = value; } }

        public IList<string> Required { get { return Schema.Required; } }

        private List<JSchemaDrop> _allOf = null;

        public IList<JSchemaDrop> AllOf
        { 
            get { return (_allOf = _allOf ?? ToDrop(Schema.AllOf)); }  
        }

        private List<JSchemaDrop> _anyOf = null;

        public IList<JSchemaDrop> AnyOf
        { 
            get { return (_anyOf = _anyOf ?? ToDrop(Schema.AnyOf)); }  
        }

        private List<JSchemaDrop> _oneOf = null;

        public IList<JSchemaDrop> OneOf
        { 
            get { return (_oneOf = _oneOf ?? ToDrop(Schema.OneOf)); }  
        }

        private JSchemaDrop _not = null;

        public JSchemaDrop Not { get { return (_not = _not ?? ToDrop(Schema.Not)); } }

        private List<string> _enum = null;

        public List<string> Enum { get { return (_enum = _enum ?? (Schema.Enum == null ? null : Schema.Enum.Select(v => v.ToString()).ToList())); } }

        public bool UniqueItems { get { return Schema.UniqueItems; } set { Schema.UniqueItems = value; } }

        public int? MinimumLength { get { return Schema.MinimumLength; } set { Schema.MinimumLength = value; } }

        public int? MaximumLength { get { return Schema.MaximumLength; } set { Schema.MaximumLength = value; } }

        public double? Minimum { get { return Schema.Minimum; } set { Schema.Minimum = value; } }

        public double? Maximum { get { return Schema.Maximum; } set { Schema.Maximum = value; } }

        public bool ExclusiveMinimum { get { return Schema.ExclusiveMinimum; } set { Schema.ExclusiveMinimum = value; } }

        public bool ExclusiveMaximum { get { return Schema.ExclusiveMaximum; } set { Schema.ExclusiveMaximum = value; } }

        public int? MinimumItems { get { return Schema.MinimumItems; } set { Schema.MinimumItems = value; } }

        public int? MaximumItems { get { return Schema.MaximumItems; } set { Schema.MaximumItems = value; } }

        public int? MinimumProperties { get { return Schema.MinimumProperties; } set { Schema.MinimumProperties = value; } }

        public int? MaximumProperties { get { return Schema.MaximumProperties; } set { Schema.MaximumProperties = value; } }

        public IDictionary<string, JToken> ExtensionData { get { return Schema.ExtensionData; } }

        public string Title { get { return Schema.Title; } set { Schema.Title = value; } }

        public string Description { get { return Schema.Description; } set { Schema.Description = value; } }

        public double? MultipleOf { get { return Schema.MultipleOf; } set { Schema.MultipleOf = value; } }

        public string Pattern { get { return Schema.Pattern; } set { Schema.Pattern = value; } }

        public IDictionary<string, Object> Dependencies { get { return Schema.Dependencies; } }

        private JSchemaDrop _additionalProperties = null;

        public JSchemaDrop AdditionalProperties { get { return (_additionalProperties = _additionalProperties ?? ToDrop(Schema.AdditionalProperties)); } }

        private Dictionary<string, JSchemaDrop> _patternProperties = null;

        public IDictionary<string, JSchemaDrop> PatternProperties { get { return (_patternProperties = _patternProperties ?? ToDrop(Schema.PatternProperties)); } }

        public bool AllowAdditionalProperties { get { return Schema.AllowAdditionalProperties; } set { Schema.AllowAdditionalProperties = value; } }

        private JSchemaDrop _additionalItems = null;

        public JSchemaDrop AdditionalItems { get { return (_additionalItems = _additionalItems ?? ToDrop(Schema.AdditionalItems)); } }

        public bool AllowAdditionalItems { get { return Schema.AllowAdditionalItems; } set { Schema.AllowAdditionalItems = value; } }

        public string Format { get { return Schema.Format; } set { Schema.Format = value; } }

        #endregion

        public string BaseClassName { get { return Schema.Title; } set { Schema.Title = value; } }

        private Dictionary<string,JSchemaDrop> _includedProperties = null;

        public Dictionary<string,JSchemaDrop> IncludedProperties
        {
            get
            { 
                return (_includedProperties = _includedProperties ?? ToDrop(Schema.GetIncludedProperties()));
            } 
        }

        public string[] IncludedPropertyNames { get { return IncludedProperties.Keys.ToArray(); } }

        private Dictionary<string, JSchemaDrop> _includedPropertyTypes = null;

        public Dictionary<string, JSchemaDrop> IncludedPropertyTypes
        { 
            get { return (_includedPropertyTypes = _includedPropertyTypes ?? ToDrop(Schema.GetIncludedProperties())); }
        }

        public JSchemaDrop(JSchema schema)
        {
            if (JSchemaPropertyNames == null)
            {
                JSchemaProperties = typeof(JSchema).GetProperties().ToArray();
                JSchemaPropertyNames = JSchemaProperties.Select(p => p.Name)
                //.Union(typeof(JSchemaDrop).GetProperties().Select(p => p.Name))
                    .ToArray(); 
                 
                var wrapCode = string.Join("\r\n",
                                   JSchemaProperties.Select(p => " public " + GetTypeName(p.PropertyType) + " " + p.Name
                                       + " {get { return Schema." + p.Name + "; }"
                                       + "  set { Schema." + p.Name + "=value; }}"));
                
                Console.WriteLine(wrapCode);
            }

            Schema = schema;
        }

        private string GetTypeName(Type t)
        {
            string baseType = t.Name;

            if (t.IsGenericType)
            {
                baseType = baseType.Substring(0, baseType.IndexOf('`'));

                baseType += "<" + string.Join(", ", t.GetGenericArguments().Select(gt => GetTypeName(gt))) + ">";
            }

            return baseType;  
        }

        public override object BeforeMethod(string method)
        {
            if (JSchemaPropertyNames.Contains(method))
                return  JSchemaProperties.First(p => p.Name == method).GetValue(Schema);
            else
                return "Undefined JSchemaDrop property: " + method;
        }
    }
} 