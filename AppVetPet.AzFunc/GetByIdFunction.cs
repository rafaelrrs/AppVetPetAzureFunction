using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AppVetPet.Infra.Repository;

namespace AppVetPet.AzFunc
{
    public static class GetByIdFunction
    {
        [FunctionName("GetByIdFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Guid id = new Guid(req.Query["id"]);

            var repository = new PetRepository();

            var pet = repository.GetById(id.ToString());

            if (pet == null)
                return new NotFoundObjectResult(new { message = "Não encontrei o agendamento" });

            return new OkObjectResult(pet);
        }
    }
}
