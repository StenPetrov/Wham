﻿
using System;
using Wham;
using Newtonsoft.Json.Schema;

namespace WhamTests
{
    public static class Schemas
    {
        public static string AddressBaseSchema = 
            @"{""title"":""Base.Address"",""type"":""object"",""properties"":{""line1"":{""type"":""string""},""line2"":{""type"":""string""},""city"":{""type"":""string""},""country"":{""type"":""string""}}}";

        public static string AddressCollectionSchema = 
            @"{""title"":""Base.AddressCollection"",""type"":""object"",""properties"":{""name"":{""type"":""string""},""addresses"":{""type"":""array"",""items"":{""$ref"":""http://wham.org/Base.Address""}}}}";

        public static string ShippingAddressSchema = 
            @"{""title"":""Base.ShippingAddress"",""allOf"":[{""$ref"":""http://wham.org/Base.Address""},{""properties"":{""type"":{""enum"":[""residential"",""business"",""null""]}},""required"":[""type""]}]}";
  
        public static JSchemaDrop ShippingAddressDrop{
            get{
                JSchema addressBase = JSchema.Parse(Schemas.AddressBaseSchema);
                JSchemaPreloadedResolver resolver = new JSchemaPreloadedResolver();
                var uri = new Uri("http://wham.org/" + addressBase.Title); 
                resolver.Add(uri, Schemas.AddressBaseSchema); 
                JSchema shippingAddress = JSchema.Parse(Schemas.ShippingAddressSchema, resolver);

                var schemaDrop = new JSchemaDrop(shippingAddress);
                return schemaDrop;
            }
        }
    }
}