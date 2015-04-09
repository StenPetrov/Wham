using System;
using NUnit.Framework;
using Wham;
using System.Text;
using DotLiquid;

namespace WhamTests
{
    [TestFixture]
    public class TagTests
    {  
        [Test]
        public void TestMultilineStringEscape()
        {
            StringBuilder sb = new StringBuilder("Single Line");
            MultilineStringEscape.EscapeAndNewlines(sb);
            Assert.AreEqual("\"Single Line\"", sb.ToString());

            sb = new StringBuilder("First Line\r\nSecond Line");
            MultilineStringEscape.EscapeAndNewlines(sb);
            Assert.AreEqual("\"First Line\"\n + \"Second Line\"", sb.ToString());

            sb = new StringBuilder(" Trim  ");
            MultilineStringEscape.EscapeAndNewlines(sb);
            Assert.AreEqual("\"Trim\"", sb.ToString());

            sb = new StringBuilder("   ");
            MultilineStringEscape.EscapeAndNewlines(sb);
            Assert.AreEqual("\"\"", sb.ToString());
        }

        [Test]
        public void TestGuidTag()
        {
            Guid guid;

            WhamEngine.InitTemplates(); 

            var g1 = Template.Parse("{% Guid 'D' %}").Render(); 
            Assert.IsNotNullOrEmpty(g1);
            Assert.IsTrue(Guid.TryParseExact(g1, "D", out guid));

            var g2 = Template.Parse("{% Guid %}").Render();
            Assert.IsNotNullOrEmpty(g2);

            Assert.AreNotSame(g1, g2);

            g1 = Template.Parse("{% Guid 'N' %}").Render(); 
            Assert.IsNotNullOrEmpty(g1);
            Assert.IsTrue(Guid.TryParseExact(g1, "N", out guid));

            Assert.Throws<Exception>(()=>Template.Parse("{% Guid 'INVALID' %}").Render());  
        }
    }
} 