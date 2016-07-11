using System;
using NUnit.Framework;
using Wham;
using DotLiquid.Exceptions;

namespace WhamTests
{
    [TestFixture]
    public class TemplateFileSystemTests
    {
        [Test]
        public void TestCSTemplateExists()
        {
            TemplateFileSystem tfs = new TemplateFileSystem();
            Assert.DoesNotThrow(() => tfs.ReadTemplateFile(new DotLiquid.Context(), "CS_ClassTemplate.dlq"));
        }

        [Test]
        public void TestInvalidTemplateThrows()
        {
            TemplateFileSystem tfs = new TemplateFileSystem();
            Assert.Throws<FileSystemException>(() => tfs.ReadTemplateFile(new DotLiquid.Context(), "SOME_TEMPLATE_THAT_DOESN'T EXIST"));
        }
    }
}

