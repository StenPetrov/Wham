
using System;

namespace WhamTests
{
    public static class Schemas
    {
        public static string AddressBaseSchema = 
            @"{""title"":""Base.Address"",""type"":""object"",""properties"":{""line1"":{""type"":""string""},""line2"":{""type"":""string""},""city"":{""type"":""string""},""country"":{""type"":""string""}}}";

        public static string AddressCollectionSchema = 
            @"{""title"":""Base.AddressCollection"",""type"":""object"",""properties"":{""name"":{""type"":""string""},""addresses"":{""type"":""array"",""items"":{""$ref"":""http://wham.org/Base.Address""}}}}";

        public static string ShippingAddressSchema = 
            @"{""title"":""Base.ShippingAddress"",""allOf"":[{""$ref"":""http://wham.org/Base.Address""},{""properties"":{""type"":{""enum"":[""residential"",""business""]}},""required"":[""type""]}]}";
 
    }
}

