using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// Generated by CS_ClassTemplate.dlq for Base.AddressCollection
namespace Wham.Base.Model
{
  public partial class AddressCollection 
  	
  {
    public static string CypherCreate {get; protected set;} = 
"MERGE (node:AddressCollection %_NODE_DATA_JSON_%)";

    public static string ToCypher(AddressCollection instance, string cypherTemplate){
    	var json = JsonConvert.SerializeObject(instance);
    	return cypherTemplate.Replace("%_NODE_DATA_JSON_%", json);
    }

      [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("addresses")]
    public List<Address> Addresses { get; set; }

  
      }
}