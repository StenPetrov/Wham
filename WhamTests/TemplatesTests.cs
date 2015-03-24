using System;
using NUnit.Framework;
using Wham;
using DotLiquid;
using Newtonsoft.Json.Schema;
using System.Linq;

namespace WhamTests
{
    [TestFixture]
    public class TemplatesTests
    {
        [Test]
        public void TestLoadCS_ClassTemplate()
        {
            Assert.IsNotNull(BuiltInTemplates.CS_ClassTemplate);

            Assert.DoesNotThrow(() =>
                DotLiquid.Template.Parse(BuiltInTemplates.CS_ClassTemplate)
            ); 
        }

        [Test]
        public void TestRegisterJSchemaType()
        {
            var accessibleProperties = typeof(JSchema).GetProperties().Select(p => p.Name).ToArray();

            Template.RegisterSafeType(typeof(JSchema), accessibleProperties); 

            var addressSchema = JSchema.Parse(Schemas.AddressBaseSchema);
            Assert.IsNotNull(addressSchema);
            Assert.IsNotNull(addressSchema.Title);

            Template template = Template.Parse("{{schema.Title}}");

            var output = template.Render(Hash.FromAnonymousObject(new { schema = addressSchema })); 
            Assert.AreEqual(addressSchema.Title, output);
        }

        [Test]
        public void TestParseJSchemaTypeWithTemplateParameters()
        {
            var accessibleProperties = typeof(JSchema).GetProperties().Select(p => p.Name).ToArray();

            Template.RegisterSafeType(typeof(JSchema), accessibleProperties); 

            var addressSchema = JSchema.Parse(Schemas.AddressBaseSchema);
            Assert.IsNotNull(addressSchema);
            Assert.IsNotNull(addressSchema.Title);

            Template template = Template.Parse("{{schema.Title}}");

            var output = template.Render(Hash.FromAnonymousObject(new { schema = addressSchema })); 
            Assert.AreEqual(addressSchema.Title, output);
        }

        [Test]
        public void TestTemplateFilters()
        {
        }

        [Test]
        public void TestAddressToCS_ClassTemplate()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true);

            var cs = wham.Liquidize(BuiltInTemplates.CS_ClassTemplate);
            Assert.IsNotNull(wham.Context.Strainer);
            Assert.IsTrue(wham.Context.Strainer.RespondTo("Namespace"));

            Assert.IsNotNull(cs);
            Assert.IsTrue(cs.IndexOf("Wham.Base") > 0);
            Assert.IsTrue(cs.IndexOf("Liquid error:") < 0);
            Assert.IsEmpty(wham.Context.Errors);
        }

        [Test]
        public void TestShippingAddressToCS_ClassTemplate()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema);
            wham.AddSchema(Schemas.ShippingAddressSchema, true);

            var cs = wham.Liquidize(BuiltInTemplates.CS_ClassTemplate);
            Assert.IsNotNull(wham.Context.Strainer);
            Assert.IsTrue(wham.Context.Strainer.RespondTo("Namespace"));

            Assert.IsNotNull(cs);
            Assert.IsTrue(cs.IndexOf("Wham.Base") > 0);
            Assert.IsTrue(cs.IndexOf("Liquid error:") < 0);
            Assert.IsEmpty(wham.Context.Errors);
        }


        [Test]
        public void TestAddressToCS_ClassTemplateProperties()
        {
            WhamEngine wham = new WhamEngine();
            wham.AddSchema(Schemas.AddressBaseSchema, true); 

            var cs = wham.Liquidize(BuiltInTemplates.CS_ClassTemplate);
           
            Assert.IsNotNull(cs);
            Assert.IsTrue(cs.IndexOf("Wham.Base") > 0);
            Assert.IsTrue(cs.IndexOf("Liquid error:") < 0);
            Assert.IsEmpty(wham.Context.Errors);
        }
    }
}

