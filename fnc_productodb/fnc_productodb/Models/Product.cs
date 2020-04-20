﻿
namespace fnc_productodb.Models
{
    using Newtonsoft.Json;
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
        public string Caracteristicas { get; set; }


    }
}
