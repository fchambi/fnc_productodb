using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json;
using fnc_productodb.Models;
using fnc_productodb.Helpers;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using System.Linq;
namespace fnc_productodb
{
    public  class IngresarProducto
    {
        [FunctionName(nameof(IngresarProducto))]
        public async Task<IActionResult> UpdateTaskItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "IngresarProducto/{id}")]HttpRequest req,
            [CosmosDB(
                        databaseName:Constants.COSMOS_DB_DATABASE_NAME,
                        collectionName:Constants.COSMO_DB_CONTAINER_NAME,
                        ConnectionStringSetting = "StrCosmos"
            )] DocumentClient client,
            ILogger logger,
            string id)
        {         
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            var updatedTask = JsonConvert.DeserializeObject<Product>(requestBody);
            Uri taskCollectionUri = UriFactory.CreateDocumentCollectionUri(Constants.COSMOS_DB_DATABASE_NAME, Constants.COSMO_DB_CONTAINER_NAME);
            var document = client.CreateDocumentQuery(taskCollectionUri,option) //obtiene todos los datos
                .Where(t => t.Id == id)//localiza al producto 
                .AsEnumerable()
                .FirstOrDefault();

            if (document == null) //Verifica que exista el producto para ingresar
            {
                logger.LogError($"TaskItem {id} not found. It may not exist!");
                return new NotFoundResult();
            }
                int dato = document.GetPropertyValue<int>("Cantidad"); //obtiene la cantidad anterior al ingreso
                dato = dato + updatedTask.Cantidad;  //suma la cantidad almacenada con la insertada
                 document.SetPropertyValue("Cantidad", dato); //cambia el valor de cantidad del producto seleccionado
 
            await client.ReplaceDocumentAsync(document);

            Product updatedTaskItemDocument = (dynamic)document;

            return new OkObjectResult(updatedTaskItemDocument);
        }
    }
}

