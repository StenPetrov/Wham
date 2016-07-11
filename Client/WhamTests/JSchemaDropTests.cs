using System;
using NUnit.Framework;
using Wham;
using DotLiquid;
using Newtonsoft.Json.Schema;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace WhamTests
{
    [TestFixture]
    public class JSchemaDropTests
    {
        public class TestDrop:Drop
        {
            public string Tested{ get { return "tested"; } }
        }
          
        [Test]
        public void TestCustomDrop()
        {
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();

            var hash = Hash.FromAnonymousObject(new { test = new TestDrop()});

            var res = Template.Parse("{{test.Tested}}").Render(hash);
            Assert.AreEqual("tested", res); 
        }

        [Test]
        public void TestSchemaProperties()
        { 
            WhamEngine.InitTemplates(); 
            var schemaDrop = Schemas.ShippingAddressDrop; 
            var hash = Hash.FromAnonymousObject(new { schema = schemaDrop});

            string res;

            res = Template.Parse("{{schema.Title}}").Render(hash);
            Assert.AreEqual(schemaDrop.Title, res);

            res = Template.Parse("{{schema.BaseClassName}}").RenderWithErrors(hash);  
            Assert.IsNotNullOrEmpty(res);
            Assert.IsFalse(res.Contains("BaseClassName"));
            Assert.AreEqual(schemaDrop.BaseClassName, res); 
        }

        [Test]
        public void TestIncludedProperties()
        { 
            WhamEngine.InitTemplates(); 
            var schemaDrop = Schemas.ShippingAddressDrop; 
            var hash = Hash.FromAnonymousObject(new { schema = schemaDrop});

            string res; 

            res = Template.Parse("{% for propName in schema.IncludedPropertyNames %}{{propName}}, {% endfor %}").RenderWithErrors(hash);  
            Assert.IsNotNullOrEmpty(res); ;
            Assert.AreEqual("line1, line2, city, country, type, ", res);  
        }

        [Test]
        public void TestIncludedPropertyTypes()
        { 
            WhamEngine.InitTemplates(); 
            var schemaDrop = Schemas.ShippingAddressDrop; 
            var hash = Hash.FromAnonymousObject(new { schema = schemaDrop});

            string res; 

            res = Template.Parse("{% for propType in schema.IncludedProperties %}{{propType.Value | FullClassName : propType.Key}}, {% endfor %}").RenderWithErrors(hash);  
            Assert.IsNotNullOrEmpty(res); 
            Assert.AreEqual("string, string, string, string, TypesEnum?, ", res);  
        }
    }
}  