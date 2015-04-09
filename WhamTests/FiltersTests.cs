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
        public void TestIsEmptyOperator()
        {
            WhamEngine.InitTemplates();
            Assert.AreEqual("EMPTY", Template.Parse("{% if NOCOLLECTION is_empty %}EMPTY{% endif %}").Render());

            Assert.AreEqual("EMPTY", Template.Parse("{% if '' is_empty %}EMPTY{% endif %}").Render());
            Assert.AreEqual("", Template.Parse("{% if 'aaa' is_empty %}EMPTY{% endif %}").Render());
        }

        [Test]
        public void TestIsRegisterClassFilter()
        {
            WhamEngine.InitTemplates();
             
            int wasCalled = 0;

            ClassNameFilters.RegisterClassCallback = new WeakReference<Action<string, string, object>>((input, name, data) =>
                {
                    wasCalled++;
                });
            
            var rc1 = Template.Parse("{{ 'OK' | RegisterClass :'X' }}").Render();

            Assert.AreEqual("OK", rc1);
            Assert.AreEqual(1, wasCalled);

            Assert.AreEqual("OK", Template.Parse("{{ 'OK' | RegisterResource :'X' }}").Render());
            Assert.AreEqual(2, wasCalled);
        }
    }
}