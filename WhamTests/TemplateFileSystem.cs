using System;
using NUnit.Framework;
using Wham;

namespace WhamTests
{
    [TestFixture]
    public class TemplateFileSystemTests
    {
        [Test]
        public void TestCSTemplateExists()
        {
            TemplateFileSystem tfs = new TemplateFileSystem(null);
            Assert.DoesNotThrow(() => tfs.ReadTemplateFile(new DotLiquid.Context(), "CS_ClassTemplate.dlq"));
        }

        [Test]
        public void TestInvalidTemplateThrows()
        {
            TemplateFileSystem tfs = new TemplateFileSystem(null);
            Assert.Throws<System.IO.FileNotFoundException>(() => tfs.ReadTemplateFile(new DotLiquid.Context(), "SOME_TEMPLATE_THAT_DOESN'T EXIST"));
        }
    }
}

