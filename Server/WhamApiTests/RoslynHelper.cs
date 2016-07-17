using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WhamApiTests
{
    public static class RoslynHelper
    {
        public static void ValidateCSFolder(string directoryPath)
        {
            try
            {
                Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories).ToList()
                    .ForEach(ValidateCSFile);
                Directory.GetFiles(directoryPath, "package.config", SearchOption.AllDirectories).ToList()
                   .ForEach(ValidatePackageFile);
                Directory.GetFiles(directoryPath, "*.csproj", SearchOption.AllDirectories).ToList()
                   .ForEach(ValidateCSProjFile);
                Directory.GetFiles(directoryPath, "*.shproj", SearchOption.AllDirectories).ToList()
                   .ForEach(ValidateCSProjFile);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate C# folder: " + directoryPath + " Error: " + x);
            }
        }

        public static void ValidatePackageFile(string packageFilePath)
        {
            try
            {
                var packageDoc = XDocument.Parse(File.ReadAllText(packageFilePath));
                Assert.IsNotNull(packageDoc.Root, "Invalid xaml file: " + packageFilePath);
                Assert.AreEqual("packages", packageDoc.Root.Name.LocalName, "Invalid packages.config file: " + packageFilePath);
            }
            catch (Exception x)
            {
                Assert.Fail("Failed to validate packages.config file: " + packageFilePath + " Error: " + x);
            }
        }

        public static void ValidateCSProjFile(string csProjFilePath)
        {
            try
            {
                var project = XDocument.Parse(File.ReadAllText(csProjFilePath));
                Assert.IsNotNull(project.Root, "Invalid xaml file: " + csProjFilePath);
                Assert.AreEqual("Project", project.Root.Name.LocalName, "Invalid CS project file: " + csProjFilePath);
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
                var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

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
    }
}
