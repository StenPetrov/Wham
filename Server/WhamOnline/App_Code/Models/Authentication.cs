﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WhamOnline.Models
{

    public class Authentication : DotLiquid.Drop
    { 
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("ClientId")]
        public string ClientId { get; set; }
    }

}
