using System;
using NUnit.Framework;
using Wham;
using Newtonsoft.Json.Schema;

namespace WhamTests
{
    [TestFixture]
    public class SchemaContextTests
    {
        [Test]
        public void TestAddressSchemaLoad()
        {
            JSchema js = null;

            Assert.DoesNotThrow(() =>
                js = JSchema.Parse(Schemas.AddressBaseSchema)
            ); 

            Assert.AreEqual("Base.Address", js.Title);
            Assert.IsFalse(js.IsAtomicType());
        }

        [Test]
        public void TestResolverAddSchema()
        {
            WhamEngine ctx = new WhamEngine();
            ctx.Resolver.Add(new Uri("http://wham.org/Base.Address"), Schemas.AddressBaseSchema);

            JSchema addressCollection = null;
            Assert.DoesNotThrow(() =>
                addressCollection = JSchema.Parse(Schemas.AddressCollectionSchema, ctx.Resolver)
            );

            Assert.AreEqual("Base.AddressCollection", addressCollection.Title);     
        }
    }
}

