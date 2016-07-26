using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WhamApiTests
{
    public static class RoslynHelper
    {
        public static void ValidateDotNetFolder(string directoryPath)
        {
            try
            {
                foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(file).ToLower();

                    if (ext != ".zip" && ext != ".request" && ext != ".error")
                    {
                        var fileContents = File.ReadAllText(file);
                        Assert.IsTrue(fileContents.IndexOf("Liquid error") < 0, "Template error in file: " + file);
                        Assert.IsTrue(fileContents.IndexOf("Liquid syntax error") < 0, "Template error in file: " + file);
                    }

                    switch (ext)
                    {
                        case ".cs": ValidateCSFile(file); break;
                        case ".config": ValidateConfigFile(file); break;
                        case ".csproj": ValidateCSProjFile(file, true); break;
                        case ".shproj": ValidateCSProjFile(file, false); break;
                        case ".xaml": ValidateCSXamlFile(file); break;
                        case ".sln": ValidateSolutionFile(file); break;
                    }
                }
            }
            catch (AssertFailedException ax)
            {
                var _exInfo = ExceptionDispatchInfo.Capture(ax);
                _exInfo.Throw();
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate C# folder: " + directoryPath + " Error: " + x);
            }
        }

        private static void ValidateSolutionFile(string file)
        {
            try
            {
                CompileSolution(file, Path.Combine(Path.GetDirectoryName(file), "bin", "Debug"));
            }
            catch (Exception x)
            {
                Assert.Fail("Can't compile solution: " + file + " \r\n Error: " + x);
            }
        }

        private static void ValidateCSXamlFile(string file)
        {
            var xamlText = File.ReadAllText(file);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(xamlText), "Empty xaml file: " + file);
            var xamlDoc = XDocument.Parse(xamlText);
            Assert.IsNotNull(xamlDoc.Root, "Invalid xaml file: " + file);
        }

        public static void ValidateConfigFile(string configFilePath)
        {
            try
            {
                var configText = File.ReadAllText(configFilePath);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(configText), "Empty packages.config file: " + configFilePath);
                var configDoc = XDocument.Parse(configText);
                Assert.IsNotNull(configDoc.Root, "Invalid xaml file: " + configFilePath);

                if (Path.GetFileNameWithoutExtension(configFilePath) == "app")
                {
                    Assert.AreEqual("configuration", configDoc.Root.Name.LocalName, "Invalid app.config file: " + configFilePath);
                }
                else if (Path.GetFileNameWithoutExtension(configFilePath) == "packages")
                {
                    Assert.AreEqual("packages", configDoc.Root.Name.LocalName, "Invalid packages.config file: " + configFilePath);
                }
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate packages.config file: " + configFilePath + " Error: " + x);
            }
        }

        public static void ValidateCSProjFile(string csProjFilePath, bool expectItemGroups)
        {
            try
            {
                var project = XDocument.Parse(File.ReadAllText(csProjFilePath));
                Assert.IsNotNull(project.Root, "Invalid xaml file: " + csProjFilePath);
                Assert.AreEqual("Project", project.Root.Name.LocalName, "Invalid CS project file: " + csProjFilePath);

                if (expectItemGroups)
                {
                    var ns = project.Root.GetDefaultNamespace();
                    var itemGroups = project.Descendants(ns + "ItemGroup").ToList();
                    Assert.IsTrue(itemGroups.Any(), "Project " + csProjFilePath + " has no <ItemGroup> elements");

                    var itemGroupWithText = itemGroups.FirstOrDefault(ig => ig.Nodes().OfType<XText>().Any());
                    Assert.IsNull(itemGroupWithText, "Project " + csProjFilePath + " has <ItemGroup> element wit text: " + itemGroupWithText);

                    var compileIncludes = itemGroups.SelectMany(ig => ig.Elements(ns + "Compile"))
                        .Union(itemGroups.SelectMany(ig => ig.Elements(ns + "EmbeddedResource")))
                        .Where(ci => ci.Attribute("Include") != null)
                        .ToList();

                    if (compileIncludes.Any())
                    {
                        var csProjPath = Path.GetDirectoryName(csProjFilePath);

                        var notFoundItems = compileIncludes
                            .Where(ci => !File.Exists(Path.Combine(csProjPath, ci.Attribute("Include").Value)))
                            .Select(ci => ci.Attribute("Include").Value)
                            .ToList();

                        Assert.IsFalse(notFoundItems.Any(), $"Project {csProjFilePath} is missing file(s): " + string.Join("\r\n", notFoundItems));
                    }
                }
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate C# project file: " + csProjFilePath + " Error: " + x);
            }
        }

        public static void ValidateXamlFile(string xamlFilePath)
        {
            try
            {
                var project = XDocument.Parse(File.ReadAllText(xamlFilePath));
                Assert.IsNotNull(project.Root, "Invalid xaml file: " + xamlFilePath);
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate xaml file: " + xamlFilePath + " Error: " + x);
            }
        }

        public static void ValidateCSFile(string filePath)
        {
            try
            {
                var csText = File.ReadAllText(filePath);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(csText), "Empty C# file: " + filePath);
                var syntaxTree = CSharpSyntaxTree.ParseText(csText);

                SyntaxNode root;
                Assert.IsTrue(syntaxTree.TryGetRoot(out root), "Can't get C# root: " + filePath);
                var diagnostics = syntaxTree.GetDiagnostics().ToList();
                var errors = diagnostics.Where(diag => diag.Severity == DiagnosticSeverity.Error).ToList();
                Assert.AreEqual(0, errors.Count, "C# erros in " + filePath + "\r\n" +
                    string.Join("\r\n ", errors.Select(err => err.Location.GetLineSpan().StartLinePosition.Line + ": " + err.GetMessage())));
                Console.WriteLine("Valid CS file: " + filePath);
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate cs file: " + filePath + " Error: " + x);
            }
        }

        public static async Task<string> FileReadAllText(string filePath)
        {
            using (var stream = File.OpenText(filePath))
            {
                return await stream.ReadToEndAsync();
            }
        }

        private static bool CompileSolution(string solutionUrl, string outputDir)
        {
            bool success = true;

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionUrl).Result;
            ProjectDependencyGraph projectGraph = solution.GetProjectDependencyGraph();
            Dictionary<string, Stream> assemblies = new Dictionary<string, Stream>();

            foreach (ProjectId projectId in projectGraph.GetTopologicallySortedProjects())
            {
                Compilation projectCompilation = solution.GetProject(projectId).GetCompilationAsync().Result;
                if (null != projectCompilation && !string.IsNullOrEmpty(projectCompilation.AssemblyName))
                {
                    using (var stream = new MemoryStream())
                    {
                        EmitResult result = projectCompilation.Emit(stream);
                        if (result.Success)
                        {
                            string fileName = string.Format("{0}.dll", projectCompilation.AssemblyName);

                            using (FileStream file = File.Create(outputDir + '\\' + fileName))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                stream.CopyTo(file);
                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
