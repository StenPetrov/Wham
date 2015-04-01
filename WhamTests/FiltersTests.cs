using System;
using NUnit.Framework;
using Wham;
using System.Text;
using DotLiquid;

namespace WhamTests
{
    [TestFixture]
    public class FiltersTests
    {
        [Test]
        public void TestClassNameFilters_Regex()
        {
            var rx = ClassNameFilters.IsValidTypeName;
            Assert.IsTrue(rx.IsMatch("_a.b"));
            Assert.IsTrue(rx.IsMatch("aaa"));
            Assert.IsTrue(rx.IsMatch("a.b_"));
            Assert.IsTrue(rx.IsMatch("_a.b.c.d.e"));
            Assert.IsFalse(rx.IsMatch("0_a.b"));
            Assert.IsFalse(rx.IsMatch(""));
            Assert.IsFalse(rx.IsMatch("_ a.b"));
            Assert.IsFalse(rx.IsMatch("-b"));
            Assert.IsFalse(rx.IsMatch("a.5b"));
            Assert.IsFalse(rx.IsMatch("a. b"));
        }

        [Test]
        public void TestClassNameFilters_Namespace()
        {
            Assert.AreEqual("Wham.a.b.c", ClassNameFilters.Namespace("a.b.c.Class"));
            Assert.AreEqual("Wham", ClassNameFilters.Namespace("Class"));
        }

        [Test]
        public void TestClassNameFilters_ClassName()
        {
            Assert.AreEqual("Class", ClassNameFilters.ClassName("a.b.c.Class"));
            Assert.AreEqual("Class", ClassNameFilters.ClassName("Class"));
        }

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
        public void TestIsEmptyOperator()
        {
            WhamEngine.InitTemplates();
            Assert.AreEqual("EMPTY", Template.Parse("{% if NOCOLLECTION is_empty %}EMPTY{% endif %}").Render());

            Assert.AreEqual("EMPTY", Template.Parse("{% if '' is_empty %}EMPTY{% endif %}").Render());
            Assert.AreEqual("", Template.Parse("{% if 'aaa' is_empty %}EMPTY{% endif %}").Render());
        }
    }
}

