using System;
using NUnit.Framework;
using DotLiquid;
using Wham;
using System.IO;
using System.Linq;

namespace WhamTests
{
    [TestFixture]
    public class FolderAndFileTests
    {
        private class NamedStream
        {
            public string Name { get; set; }

            public string WrittenContents { get; set; }
        }

        private class NamedStreams
        {
            public string Name { get { return Items[0].Name; } }

            public string WrittenContents { get { return Items[0].WrittenContents; } }

            public System.Collections.Generic.List<NamedStream> Items { get; private set; } = new System.Collections.Generic.List<NamedStream>();
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

        static NamedStreams SetupFakeOutput()
        {
            Wham.WhamEngine.InitTemplates();
          
            var namedStreams = new NamedStreams();

            ((TemplateFileSystem)Template.FileSystem).FCreateOutputStream = fn =>
            { 
                return new MemoryStream();
            };

            ((TemplateFileSystem)Template.FileSystem).FileEvent += (s, e) =>
            {
                Console.WriteLine("[IVAUDJHFGQK] File written: " + e.Name);

                var namedStream = new NamedStream()
                {
                    Name = e.Name,
                    WrittenContents = System.Text.Encoding.UTF8.GetString(((MemoryStream)e.Stream).GetBuffer(), 0, (int)((MemoryStream)e.Stream).Length)
                };
                    
                namedStreams.Items.Add(namedStream);
            };

            return namedStreams;
        }

        [Test]
        public void TestFileOutput()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %} {% File 'outFile.txt' %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);
            Assert.IsNotNull(namedStream.WrittenContents); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("outFile.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputFromVariable()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %}{% assign fn='file.txt'%}{% File fn %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);  
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("file.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputFromVariableWithFilter()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %}{% assign fn = 'file' | Append: '.txt' %}{% File fn %}{{ output.file }}{% endFile %} {% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name);  
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.AreEqual("file.txt", namedStream.WrittenContents);
        }

        [Test]
        public void TestFileOutputWithInclude()
        { 
            var namedStream = SetupFakeOutput();
            var fileParsed = Template.Parse("{% Folder Test %} {% assign csFile = schema | BaseClassFullName | Append: '.cs' -%} {% File csFile %}{% include 'CS_ClassTemplate.dlq'-%}{% endFile %}{% endFolder %}").Render();

            Assert.IsNotNull(namedStream.Name); 
            Assert.IsTrue(string.IsNullOrWhiteSpace(fileParsed));
            Assert.IsTrue(namedStream.WrittenContents.StartsWith("using"));
        }

        private static string MasterTemplate =
            @"{% Folder 'WhAM' -%}
{% for jSchema in schemas -%}
  {% assign schema = jSchema -%}
  {% Folder 'Model' -%}
    {% assign csFile = schema | ClassName | Append: '.cs' -%}
    {{ csFile }}
    {% File csFile -%}{% include 'CS_ClassTemplate.dlq' -%}{% endFile -%}
  {% endFolder -%} 
{% endfor -%}
{% endFolder -%}";

        [Test]
        public void TestMasterCSTemplate()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true);

            var namedStream = SetupFakeOutput();

            var masterResult = wham.Liquidize(MasterTemplate); 
 
            Assert.IsTrue(wham.Context.Errors == null || wham.Context.Errors.Count == 0);
            Assert.IsNotNullOrEmpty(masterResult);
            masterResult = masterResult.Trim();
            Assert.AreEqual("address.cs", masterResult.ToLower());
            Assert.IsNotNull(namedStream.Name);
            Assert.IsTrue(namedStream.Name.ToLower().EndsWith("address.cs"));
            Assert.IsNotNull(namedStream.WrittenContents);  
            Assert.IsTrue(namedStream.WrittenContents.StartsWith("using"));
        }

        [Test]
        public void TestWhamMasterTemplate()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true);
            wham.AddSchema(Schemas.ShippingAddressSchema, true);
            wham.AddSchema(Schemas.AddressCollectionSchema, true);

            var namedStream = SetupFakeOutput();

            var masterResult = wham.Liquidize(BuiltInTemplates.WhamMasterTemplate); 

            var errors = ("" + string.Join("\r\n", wham.Context.Errors.Select(e => e.ToString()))).Trim();
            Assert.AreEqual(string.Empty, errors);
           
            Assert.IsNotNullOrEmpty(masterResult);
            masterResult = masterResult.Trim();
            Assert.AreEqual(string.Empty, masterResult.ToLower());

            Assert.IsTrue(namedStream.Items.Count >= 4);
            Console.WriteLine(string.Join("\r\n", namedStream.Items.Select(i => i.Name)));

            Assert.IsNotNull(namedStream.Items.First().Name);
            Assert.IsTrue(namedStream.Items.First().Name.ToLower().EndsWith("address.cs"));
            Assert.IsNotNull(namedStream.Items.First().WrittenContents);  
            Assert.IsTrue(namedStream.Items.First().WrittenContents.StartsWith("using"));
 
            Assert.IsNotNull(namedStream.Items.Any(s => s.Name.ToLower().EndsWith(".csproj")));
            Assert.IsNotNull(namedStream.Items.Single(s => s.Name.ToLower().EndsWith(".config")));
            Assert.AreEqual(3, namedStream.Items.Count(s => s.WrittenContents.StartsWith("<?xml")));
        }

        [Test]
        public void TestWhamMasterTemplateWithFileOutput()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true);
            wham.AddSchema(Schemas.ShippingAddressSchema, true);
            wham.AddSchema(Schemas.AddressCollectionSchema, true);
             
            var masterResult = wham.Liquidize(BuiltInTemplates.WhamMasterTemplate); 
            Console.WriteLine(masterResult.Trim());

            var errors = ("" + string.Join("\r\n", wham.Context.Errors.Select(e => e.ToString()))).Trim();
            Console.WriteLine(errors);
            Assert.AreEqual(string.Empty, errors);  
        }
    }
}