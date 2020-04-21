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
using System.Collections.Generic;
namespace fnc_productodb
{
    public class ConsultarProducto
    {
        [FunctionName(nameof(ConsultarProducto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"get", Route = "ConsultarProducto/{id}")] HttpRequest req,
            [CosmosDB(
                        databaseName:Constants.COSMOS_DB_DATABASE_NAME,
                        collectionName:Constants.COSMO_DB_CONTAINER_NAME,
                        ConnectionStringSetting = "StrCosmos",
                        SqlQuery ="SELECT * FROM c where c.id={id}" //En este caso ejecutamos una sentencia SQL
            )]IEnumerable<Product> productItem,
            ILogger log,string id)
        {
            
            if (productItem == null) //Verifica que exista el producto que consultamos
            {
                log.LogError($"Producto {id} not found. It may not exist!");
                return new NotFoundResult();
            }    
            return new OkObjectResult(productItem);
        }
    }
}
