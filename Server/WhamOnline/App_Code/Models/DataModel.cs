﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace WhamOnline.Models
{ 
    public class DataModel : DotLiquid.Drop
    { 
        [JsonProperty("TableName")]
        public string TableName { get; set; }

        [JsonProperty("isVisible", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(true)] 
        public bool IsVisible { get; set; }

        [JsonProperty("fields")]
        public Field[] Fields { get; set; }
    }

}
