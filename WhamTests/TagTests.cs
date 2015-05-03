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
            MultilineStringEscapeTag.EscapeAndNewlines(sb);
            Assert.AreEqual("\"Single Line\"", sb.ToString());

            sb = new StringBuilder("First Line\r\nSecond Line");
            MultilineStringEscapeTag.EscapeAndNewlines(sb);
            Assert.AreEqual("\"First Line\"\n + \"Second Line\"", sb.ToString());

            sb = new StringBuilder(" Trim  ");
            MultilineStringEscapeTag.EscapeAndNewlines(sb);
            Assert.AreEqual("\"Trim\"", sb.ToString());

            sb = new StringBuilder("   ");
            MultilineStringEscapeTag.EscapeAndNewlines(sb);
            Assert.AreEqual("\"\"", sb.ToString());

            var t = Template.Parse("{% MultilineStringEscape %} \r\n  {% endMultilineStringEscape %}").Render(); 
            Assert.IsNullOrEmpty(t); 

            t = Template.Parse("{% MultilineStringEscape %} XXX  {% endMultilineStringEscape %}").Render(); 
            Assert.IsNotNullOrEmpty(t); 
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

            Assert.Throws<Exception>(() => Template.Parse("{% Guid 'INVALID' %}").Render());  
        }

        [Test]
        public void TestTrimTag()
        { 
            WhamEngine.InitTemplates(); 

            var t = Template.Parse("{% Trim %}   {% endTrim %}").Render(); 
            Assert.IsNullOrEmpty(t); 

            t = Template.Parse("{% Trim %}  NOT EMPTY {% endTrim %}").Render(); 
            Assert.IsNotNullOrEmpty(t); 
            Assert.AreEqual("NOT EMPTY", t);      
        }
    }
} 