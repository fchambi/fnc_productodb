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
using Microsoft.Azure.Documents.Client;
using System.Linq;
namespace fnc_productodb
{
    public  class SacarProducto
    {

        [FunctionName(nameof(SacarProducto))]
        public async Task<IActionResult> UpdateTaskItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "SacarProducto/{id}")]HttpRequest req,
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

            var document = client.CreateDocumentQuery(taskCollectionUri, option)
                .Where(t => t.Id == id)
                .AsEnumerable()
                .FirstOrDefault();

            if (document == null)
            {
                logger.LogError($"Producto {id} not found. It may not exist!");
                return new NotFoundResult();
            }
            int dato = document.GetPropertyValue<int>("Cantidad");
            if (dato < updatedTask.Cantidad)
            {
                logger.LogError($"Producto {id} no tiene suficiente cantidad para ese retiro");
                return new NotFoundResult();
            }
            dato = dato - updatedTask.Cantidad;
            document.SetPropertyValue("Cantidad", dato);


            await client.ReplaceDocumentAsync(document);

            Product updatedTaskItemDocument = (dynamic)document;

            return new OkObjectResult(updatedTaskItemDocument);
        }
    }
}
