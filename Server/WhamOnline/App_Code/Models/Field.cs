﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WhamOnline.Models
{

    public class Field : DotLiquid.Drop
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("refList")]
        public string[] RefList { get; set; }

        [JsonProperty("uiType")]
        public string UiType { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("isAuth")]
        public string IsAuth { get; set; }
    }

}