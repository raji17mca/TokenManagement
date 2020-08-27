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
        private readonly string queryString = "SELECT * FROM C";

        public TokenCosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(CustomerDetails customer)
        {
            int count = GetBankStaffTokenDetails(this.queryString).Count();

            customer.TokenNumber = ++count;
            customer.Status = "In Queue";
            await this._container.CreateItemAsync<CustomerDetails>(customer, new PartitionKey(customer.Id));
        }

        public IEnumerable<CustomerTokenDetails> GetCustomerTokenDetails(string queryString)
        {
            var query = this._container.GetItemQueryIterator<CustomerTokenDetails>(new QueryDefinition(queryString));
            List<CustomerTokenDetails> results = new List<CustomerTokenDetails>();
            while (query.HasMoreResults)
            {
                var response = query.ReadNextAsync();

                results.AddRange(response.Result);
            }

            return results;
        }

        public IEnumerable<BankStaffTokenDetails> GetBankStaffTokenDetails(string queryString)
        {
            var query = this._container.GetItemQueryIterator<BankStaffTokenDetails>(new QueryDefinition(queryString));
            List<BankStaffTokenDetails> results = new List<BankStaffTokenDetails>();
            while (query.HasMoreResults)
            {
                var response =  query.ReadNextAsync();

                results.AddRange(response.Result);
            }

            return results;
        }

        public void UpdateItemAsync(string id, CustomerDetails customerDetails)
        {
            this._container.UpsertItemAsync<CustomerDetails>(customerDetails, new PartitionKey(id));
        }
    }
}
