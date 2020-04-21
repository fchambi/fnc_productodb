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
using fnc_productodb.Helpers;

namespace fnc_productodb
{
    public  class CrearProducto
    {
        [FunctionName(nameof(CrearProducto))]
        public async Task<IActionResult>  Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                        databaseName:Constants.COSMOS_DB_DATABASE_NAME,
                        collectionName:Constants.COSMO_DB_CONTAINER_NAME,
                        ConnectionStringSetting = "StrCosmos"
            )]  IAsyncCollector <object> products,
            ILogger log)
        {

            IActionResult returnvalue = null;
            try
            {
                
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Product>(requestBody);
                var product = new Product
                {
                    ProductId = data.ProductId,
                    Nombre = data.Nombre,
                    Precio = data.Precio,
                    Caracteristicas = data.Caracteristicas,
                    Cantidad = data.Cantidad
                };
                await products.AddAsync(product);
                
                log.LogInformation($"Item creado {product.Nombre}");
                returnvalue = new OkObjectResult(product);
            }
            catch (Exception ex)
            {
                log.LogError($"No se creo el producto. EXception:{ex.Message} ");
                returnvalue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
                return returnvalue;
            
        }
    }
}
