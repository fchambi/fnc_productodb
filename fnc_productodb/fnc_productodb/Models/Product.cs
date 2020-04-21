
using System;
using System.Collections.Generic;
using System.Text;
using fnc_productodb.Models;
namespace fnc_productodb.Models
{
    using Microsoft.AspNetCore.JsonPatch.Adapters;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    
    public class Product
    {

        [JsonProperty("id")]
        public string ProductId { get; set; }
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
        [JsonProperty("Precio")]
        public double Precio { get; set; }
        [JsonProperty("Cantidad")]
        public int Cantidad { get; set; }
        [JsonProperty("Caracteristicas")]
        public List<object> Caracteristicas { get; set; }
  
    }
}
