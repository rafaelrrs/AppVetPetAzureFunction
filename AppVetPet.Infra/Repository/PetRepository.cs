using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVetPet.Infra;
using Dapper;
using Microsoft.Azure.Cosmos;

namespace AppVetPet.Infra.Repository
{
    public class PetRepository
    {
        private string ConnectionString = "AccountEndpoint=;";
        private string Container = "vetpetdb";
        private string Database = "asvdb";

        private CosmosClient CosmosClient { get; set; }

        public PetRepository()
        {
            this.CosmosClient = new CosmosClient(this.ConnectionString);
        }

        public List<Pet> GetAll()
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

            var result = new List<Pet>();

            var queryResult = container.GetItemQueryIterator<Pet>(queryDefinition);

            while (queryResult.HasMoreResults)
            {
                FeedResponse<Pet> currentResultSet = queryResult.ReadNextAsync().Result;
                result.AddRange(currentResultSet.Resource);
            }

            return result;
        }

        public Pet GetById(string id)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.id = '{id}'");

            var queryResult = container.GetItemQueryIterator<Pet>(queryDefinition);

            Pet item = null;

            while (queryResult.HasMoreResults)
            {
                FeedResponse<Pet> currentResultSet = queryResult.ReadNextAsync().Result;
                item = currentResultSet.Resource.FirstOrDefault();
            }

            return item;
        }

        public async Task Save(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.CreateItemAsync<Pet>(item, new PartitionKey(item.SKU));
        }

        public async Task Update(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.ReplaceItemAsync<Pet>(item, item.Id.ToString(), new PartitionKey(item.SKU));
        }

        public async Task Delete(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.DeleteItemAsync<Pet>(item.Id.ToString(), new PartitionKey(item.SKU));
        }
    }
}
