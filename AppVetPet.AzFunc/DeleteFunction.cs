using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AppVetPet.Infra;
using AppVetPet.Infra.Repository;

namespace AppVetPet.AzFunc
{
    public static class DeleteFunction
    {
        [FunctionName("DeleteFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Pet data = JsonConvert.DeserializeObject<Pet>(requestBody);

            if (data == null)
                return new BadRequestObjectResult(new { message = "Dados para criação de uma tarefa é obrigatoria" });


            var repository = new PetRepository();

            data.Id = Guid.NewGuid().ToString();

            await repository.Save(data);

            return new CreatedResult("", data);
        }
    }
}
