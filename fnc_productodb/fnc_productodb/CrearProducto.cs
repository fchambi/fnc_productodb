using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using fnc_productodb.Models;
namespace fnc_productodb
{
    public  class CrearProducto
    {
        [FunctionName(nameof(CrearProducto))]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Product>(requestBody);
            var product = new Product
            {
                ProductId = data.ProductId,
                Nombre = data.Nombre,
                Precio = data.Precio,
                Caracteristicas=data.Caracteristicas,
                Cantidad=data.Cantidad
            };

            string responseMessage = product.Nombre;
            return new OkObjectResult(responseMessage);
        }
    }
}
