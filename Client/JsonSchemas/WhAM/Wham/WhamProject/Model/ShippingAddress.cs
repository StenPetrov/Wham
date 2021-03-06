using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// Generated by CS_ClassTemplate.dlq for Base.ShippingAddress
namespace Wham.Base.Model
{
  public partial class ShippingAddress 
  	: Wham.Base.Model.Address
  {
    public static string CypherCreate {get; protected set;} = 
"MERGE (node:Wham.Base.Address %_NODE_DATA_JSON_%)"
 + "  SET node :ShippingAddress";

    public static string ToCypher(ShippingAddress instance, string cypherTemplate){
    	var json = JsonConvert.SerializeObject(instance);
    	return cypherTemplate.Replace("%_NODE_DATA_JSON_%", json);
    }

      [JsonProperty("line1")]
    public string Line1 { get; set; }

    [JsonProperty("line2")]
    public string Line2 { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("type")]
    public TypesEnum? Type { get; set; }

  
    	public enum TypesEnum { 
  	    	  residential,  
  	    	  business,  
  	    	  
  	  }
      }
}