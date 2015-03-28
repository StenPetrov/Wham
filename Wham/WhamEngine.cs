using System;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using DotLiquid;
using System.Linq;

namespace Wham
{
    public class WhamEngine
    {
        public JSchema CurrentSchema{ get; set; }

        public JSchemaPreloadedResolver Resolver { get; private set; } = new JSchemaPreloadedResolver();

        public Context Context { get; set; }

        public RenderParameters RenderParameters{ get; set; }

        public Uri AddSchema(string jsonSchema, bool makeCurrent = false)
        {
            JSchema jschema = JSchema.Parse(jsonSchema, Resolver);

            if (!string.IsNullOrEmpty(jschema.Title))
            {
                if (ClassNameFilters.IsValidTypeName.IsMatch(jschema.Title))
                {
                    if (makeCurrent)
                        CurrentSchema = jschema;
                    
                    var uri = new Uri("http://wham.org/" + jschema.Title); 
                    Resolver.Add(uri, jsonSchema);

                    return uri;
                }
                else
                    throw new Exception("[FIAIQIWEHRH] Invalid schema title, it needs to be in namespace.class format");
            }
            else
                throw new Exception("[FAUHQRJKWRTO] Schema needs Title");
        }

        public string Liquidize(string template)
        {
            if (Context == null)
                Context = new Context();

            Context["schema"] = new JSchemaDrop(CurrentSchema);

            if (RenderParameters == null)
            {
                RenderParameters = new RenderParameters
                { 
                    Context = Context,
                    Filters = new[]
                    { 
                        typeof(ClassNameFilters),
                    },
                    LocalVariables = Hash.FromAnonymousObject(new {
                            schema = new JSchemaDrop(CurrentSchema),
                    }),
                    RethrowErrors = true,
                }; 
            } 

            InitTemplates();

            var parsedTemplate = Template.Parse(template); 

            return parsedTemplate.Render(RenderParameters); 
        }

        public static void InitTemplates()
        {  
            DotLiquidExtensions.RegisterSafeType(typeof(JSchemaDrop));
            DotLiquidExtensions.RegisterSafeType(typeof(KeyValuePair<string, string>));
            DotLiquidExtensions.RegisterSafeType(typeof(KeyValuePair<string, JSchemaDrop>));
            DotLiquidExtensions.RegisterSafeType(typeof(List<JSchemaDrop>));
                
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();

            Template.RegisterFilter(typeof(ClassNameFilters));
            Template.RegisterFilter(typeof(CollectionFilters));

            Template.RegisterTag<ClassEnums>("ClassEnums");
        }
    }
}

