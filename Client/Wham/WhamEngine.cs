using System;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using DotLiquid;
using System.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;
using System.IO;

namespace Wham
{
    public class WhamEngine
    {
        public JSchema CurrentSchema { get; set; }

        public JSchemaPreloadedResolver Resolver { get; private set; } = new JSchemaPreloadedResolver ();

        public Context Context { get; set; } = new Context ();

        public RenderParameters RenderParameters { get; set; }

        public WhamEngine (string outputRootFolder = null)
        {
            InitEngine ();
            Context.GetTracer ().Info ("WhamEngine stared");

            if (outputRootFolder == null) {
                var asm = Assembly.GetEntryAssembly () ?? Assembly.GetExecutingAssembly ();
                outputRootFolder = Path.Combine (Path.GetDirectoryName (asm.Location), "OUTPUT");
            }

            Context [FolderTag.FileAndFolderOutputHash] =
                    FolderTag.CreateCurrentOutputHash (outputRootFolder, null, null, null);

        }

        public Uri AddSchema (string jsonSchema, bool makeCurrent = false)
        {
            JSchema jschema = JSchema.Parse (jsonSchema, Resolver);

            Context.GetTracer ().Info ($"WhamEngine adding schema {jschema.Title}");
            if (!string.IsNullOrEmpty (jschema.Title)) {
                if (ClassNameFilters.IsValidTypeName.IsMatch (jschema.Title)) {
                    if (makeCurrent)
                        CurrentSchema = jschema;

                    var uri = new Uri ("http://wham.org/" + jschema.Title);
                    Resolver.Add (uri, jsonSchema);

                    if (Context != null) {
                        Context ["currentSchema"] = new JSchemaDrop (CurrentSchema);
                        var schemas = Context ["schemas"] as List<JSchemaDrop> ?? new List<JSchemaDrop> ();
                        schemas.Add (new JSchemaDrop (jschema));
                        Context ["schemas"] = schemas;
                    }

                    return uri;
                } else
                    throw new WhamException ("[FIAIQIWEHRH] Invalid schema title, it needs to be in namespace.class format");
            } else
                throw new WhamException ("[FAUHQRJKWRTO] Schema needs Title");
        }

        public string Liquidize (string template)
        {
            Context ["schema"] = new JSchemaDrop (CurrentSchema);

            Context.GetTracer ().Info ("[WHEGNJHKOQPA] Liquidizing template: " + template);
            if (RenderParameters == null) {
                RenderParameters = new RenderParameters {
                    Context = Context,
                    Filters = new []
                    {
                        typeof(ClassNameFilters),
                    },
                    LocalVariables = Hash.FromAnonymousObject (new {
                        schema = new JSchemaDrop (CurrentSchema),
                    }),
                    RethrowErrors = true,
                };
            }

            var parsedTemplate = Template.Parse (template);

            return parsedTemplate.Render (RenderParameters);
        }

        public static void InitEngine ()
        {
            DotLiquidExtensions.RegisterSafeType (typeof (JSchemaDrop));
            DotLiquidExtensions.RegisterSafeType (typeof (KeyValuePair<string, string>));
            DotLiquidExtensions.RegisterSafeType (typeof (KeyValuePair<string, JSchemaDrop>));
            DotLiquidExtensions.RegisterSafeType (typeof (List<JSchemaDrop>));

            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention ();

            Condition.Operators ["is_empty"] = (left, right) => {
                string asString = left as string ?? right as string;
                if (!string.IsNullOrEmpty (asString))
                    return false;

                IEnumerable enu = left as IEnumerable ?? right as IEnumerable;
                if (enu != null && enu.OfType<object> ().Any ())
                    return false;

                return true;
            };

            Template.RegisterFilter (typeof (ClassNameFilters));
            Template.RegisterFilter (typeof (CollectionFilters));

            Template.RegisterTag<ClassEnumsTag> ("ClassEnums");
            Template.RegisterTag<GuidTag> ("Guid");
            Template.RegisterTag<MultilineStringEscapeTag> ("MultilineStringEscape");
            Template.RegisterTag<FolderTag> ("Folder");
            Template.RegisterTag<FileTag> ("File");
            Template.RegisterTag<TrimTag> ("Trim");
            Template.RegisterTag<TraceTag> ("Trace");

            Template.FileSystem = new TemplateFileSystem ();

        }
    }
}