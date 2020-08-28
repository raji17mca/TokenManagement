using Microsoft.Azure.Cosmos;
using System;
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
            int count = this._container.GetItemLinqQueryable<CustomerDetails>(true).AsEnumerable().Count();

            customer.Id = Guid.NewGuid().ToString();
            customer.TokenNumber = ++count;
            customer.Status = "In Queue";

            await this._container.CreateItemAsync<CustomerDetails>(customer, new PartitionKey(customer.Id));
        }

        public IEnumerable<CustomerTokenDetails> GetCustomerTokenDetails(string queryString)
        {
            var query = this._container.GetItemQueryIterator<CustomerDetails>(new QueryDefinition(queryString));
            List<CustomerDetails> results = new List<CustomerDetails>();
            List<CustomerTokenDetails> customerTokenDetailsList = new List<CustomerTokenDetails>();
            while (query.HasMoreResults)
            {
                var response = query.ReadNextAsync().Result;

                results.AddRange(response);
            }

            //// results = (List<CustomerDetails>)results.Where(x => x.Status != "Served");
            ////results.ForEach(x => customerTokenDetails.Add(new CustomerTokenDetails
            ////{
            ////    EstimatedWaitingTime 
            ////}));
            ///

            int bankTransactionQueue = 0;
            int serviceQueue = 0;
            foreach (var item in results)
            {
                var customerTokenDetails = new CustomerTokenDetails {
                    Counter = item.Counter,
                    TokenNumber = item.TokenNumber,
                    ServiceType = item.ServiceType
                };

                if (item.Status == "In Queue")
                {
                    if (item.ServiceType == "Bank transaction")
                    {
                        ++bankTransactionQueue;
                        customerTokenDetails.EstimatedWaitingTime = 5 * bankTransactionQueue;

                    }

                    if (item.ServiceType == "Service")
                    {
                        ++serviceQueue;
                        customerTokenDetails.EstimatedWaitingTime = 25 * serviceQueue;

                    }
                }

                customerTokenDetailsList.Add(customerTokenDetails);
            }

            return customerTokenDetailsList;
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

        public async Task UpdateItemAsync(string id, string status)
        {
            var customer = this._container.GetItemLinqQueryable<CustomerDetails>(true).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            if(customer != null)
            {
                customer.Counter = customer.ServiceType == "Bank transaction" ? 1 : 2;
                customer.Status = status;
            }

            await this._container.UpsertItemAsync<CustomerDetails>(customer, new PartitionKey(id));
        }
    }
}
