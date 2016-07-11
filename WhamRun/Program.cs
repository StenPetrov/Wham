using System;
using System.Linq;
using System.IO;
using Wham;
using System.Reflection;

namespace WhamRun
{
    class Program
    {

        // $ mono whamrun.exe /Users/Sten/Documents/Projects/Wham/JsonSchemas

        public static void Main(string[] args)
        { 
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e.Exception is TargetInvocationException)
                {
                    Console.WriteLine("[TGIAODIFQWR] Target Invocation Exception: " + e.Exception.Message);
                }
                else
                    Console.WriteLine("[ATIOAEIRJQW] Exception: " + e.Exception);
            }; 

            if (args == null || !args.Any())
            {
                Console.WriteLine("Usage: WhamRun inputFolder [outputFolder]\r\n inputFolder must contain JSON Schema files");
            }
            else
            {
                if (Directory.Exists(args[0]))
                {
                    Directory.SetCurrentDirectory(args[0]);

                    var files = Directory.GetFiles(args[0]);

                    var schemas = files
                        .Select(f => new Tuple<string,string>(f, File.ReadAllText(f)))
                        .Where(ft => ft.Item2.Contains("\"title\""))
                        .Select(t => new SchemaItem{ FileName = t.Item1, Content = t.Item2 })
                        .ToList();

                    WhamEngine engine = new WhamEngine();
                     
                    while (schemas.Any())
                    {
                        var added = schemas
                            .Where(s => string.IsNullOrEmpty(s.Uri))
                            .Where(s =>
                            {
                                try
                                {   
                                    var uri = engine.AddSchema(s.Content);
                                    s.Uri = uri.AbsolutePath;
                                    return !string.IsNullOrEmpty(s.Uri);
                                }
                                catch (Exception sx)
                                {
                                    s.Error = sx;
                                }

                                return false;
                            })
                            .ToList();
                        if (!added.Any())
                            break;
                    } 

                    if (schemas.Any(s => string.IsNullOrEmpty(s.Uri)))
                    {
                        Console.WriteLine("Not all schemas can be added");
                        Console.WriteLine(String.Join("\r\n",
                                schemas.Where(s => string.IsNullOrEmpty(s.Uri))
                                .Select(s => "  File: " + s.FileName + "  ERROR: " + s.Error)));
                    }
                    else
                    {
                        Console.WriteLine("All schemas loaded.");
                        var outPath = args.Skip(1).FirstOrDefault() ?? Path.Combine(args.First(), "WhAM");
                        if (!Directory.Exists(outPath))
                            Directory.CreateDirectory(outPath);
                        Directory.SetCurrentDirectory(outPath);

                        Console.WriteLine("Wham output to: " + outPath);
                        var result = ("" + engine.Liquidize(BuiltInTemplates.WhamMasterTemplate)).Trim();

                        Console.WriteLine(result);

                        if (engine.Context.Errors != null && engine.Context.Errors.Any())
                        {
                            Console.WriteLine("dotLIQUID ERRORS:");
                            Console.WriteLine(string.Join(System.Environment.NewLine,
                                    engine.Context.Errors.Select(e => " - Error: " + e.Message + "  " + e.InnerException)));
                        }

                        Console.WriteLine("Done.");
                    }
                }
                else
                {
                    Console.WriteLine("input folder must exist"); 
                }
            }
        }
    }
}

public class SchemaItem
{
    public string Uri { get; set; }

    public string Content { get; set; }

    public Exception Error { get; set; }

    public string FileName{ get; set; }
}