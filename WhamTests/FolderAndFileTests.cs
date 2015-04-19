using System;
using NUnit.Framework;
using DotLiquid;
using Wham;
using System.IO;

namespace WhamTests
{
    [TestFixture]
    public class FolderAndFileTests
    {
        private class NamedStream
        {
            public string Name { get; set; }

            public Stream Stream { get; set; }

            public string WrittenContents { get; set; }
        }

        [Test]
        public void TestFolderVariables()
        {
            Wham.WhamEngine.InitTemplates();
               
            Assert.AreEqual("Test", Template.Parse("{% Folder 'Test' %}{{ output.lastFolder }}{% endFolder %}").Render()); 
            var parentFolder = Template.Parse("{% Folder 'Test' %}{{ output.parentFolder }}{% endFolder %}").Render();
            var fullFolder = Template.Parse("{% Folder 'Test' %}{{ output.folder }}{% endFolder %}").Render();
            Assert.IsNotNullOrEmpty(fullFolder);
            Assert.IsNotNullOrEmpty(parentFolder);
            Assert.AreEqual(Path.Combine(parentFolder, "Test"), fullFolder);
        }

        static NamedStream SetupFakeOutput()
        {
            Wham.WhamEngine.InitTemplates();
          
            var namedStream = new NamedStream();

            ((TemplateFileSystem)Template.FileSystem).FCreateOutputStream = fn =>
            {
                namedStream.Name = fn;
                return (namedStream.Stream = new MemoryStream());
            };

            ((TemplateFileSystem)Template.FileSystem).FileEvent += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("[IVAUDJHFGQK] File written: " + e.Name);

                namedStream.WrittenContents = 
                        System.Text.Encoding.UTF8.GetString(((MemoryStream)e.Stream).GetBuffer(), 0, (int)((MemoryStream)e.Stream).Length);
            };

            return namedStream;
        }

        [Test]
        public void TestFileOutput()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %} {% File 'outFile.txt' %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);
            Assert.IsNotNull(namedStream.Stream); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("outFile.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputFromVariable()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %}{% assign fn='file.txt'%}{% File fn %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);
            Assert.IsNotNull(namedStream.Stream); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("file.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputFromVariableWithFilter()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %}{% assign fn = 'file' | Append: '.txt' %}{% File fn %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);
            Assert.IsNotNull(namedStream.Stream); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("file.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputWithInclude()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %} {% assign csFile = schema | BaseClassFullName | Append: '.cs' -%} {% File csFile %}{% include 'CS_ClassTemplate.dlq'-%}{% endFile %}{% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);
            Assert.IsNotNull(namedStream.Stream); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.IsTrue(namedStream.WrittenContents.StartsWith("using"));
        }

        [Test]
        public void TestAddressToCS_ClassTemplate()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true);

            var namedStream = SetupFakeOutput();

            var masterResult = wham.Liquidize(BuiltInTemplates.WhamMasterTemplate); 
 
            Assert.IsTrue(wham.Context.Errors == null || wham.Context.Errors.Count == 0);
            Assert.IsNotNullOrEmpty(masterResult);
            masterResult = masterResult.Trim();
            Assert.AreEqual("address.cs", masterResult.ToLower());
            Assert.IsNotNull(namedStream.Name);
            Assert.IsTrue(namedStream.Name.ToLower().EndsWith("address.cs"));
            Assert.IsNotNull(namedStream.Stream);  
            Assert.IsTrue(namedStream.WrittenContents.StartsWith("using"));
        }
    }
}