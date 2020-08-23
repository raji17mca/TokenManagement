using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManagementSystem.Models;

namespace TokenManagementSystem.Services
{
    public class TokenCosmosDbService : ITokenCosmosDBService
    {
        private Container _container;

        public TokenCosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        
        public async Task AddItemAsync(TokenDetails tokenDetails)
        {
            await this._container.CreateItemAsync<TokenDetails>(tokenDetails, new PartitionKey(tokenDetails.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<TokenDetails>(id, new PartitionKey(id));
        }

        public async Task<TokenDetails> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<TokenDetails> response = await this._container.ReadItemAsync<TokenDetails>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            { 
                return null;
            }

        }

        public async Task<IEnumerable<TokenDetails>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<TokenDetails>(new QueryDefinition(queryString));
            List<TokenDetails> results = new List<TokenDetails>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, TokenDetails tokenDetails)
        {
            await this._container.UpsertItemAsync<TokenDetails>(tokenDetails, new PartitionKey(id));
        }
    }
}
