﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WhamOnline.Models
{ 
    public class AppGenConfig : DotLiquid.Drop
    { 
        [JsonProperty("AppOptions")]
        public AppOptions AppOptions { get; set; }

        [JsonProperty("Owner")]
        public Owner Owner { get; set; }

        [JsonProperty("Database")]
        public Database Database { get; set; }

        [JsonProperty("Authentication")]
        public Authentication Authentication { get; set; }

        [JsonProperty("DataModel")]
        public DataModel[] DataModel { get; set; }
    } 
}