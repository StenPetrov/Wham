using System;
using NUnit.Framework;
using Wham;

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
    }
}

