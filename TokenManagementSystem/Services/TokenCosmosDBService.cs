using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManagementSystem.Constants;
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

        public async Task<int> AddItemAsync(CustomerDetails customer)
        {
            int count = this._container.GetItemLinqQueryable<CustomerDetails>(true).AsEnumerable().Count();

            customer.Id = Guid.NewGuid().ToString();
            customer.TokenNumber = ++count;
            customer.Status = Status.InQueue;

            await this._container.CreateItemAsync<CustomerDetails>(customer, new PartitionKey(customer.Id));
            return customer.TokenNumber;
        }


        public async Task UpdateItemAsync(string id, string status)
        {
            var customer = this._container.GetItemLinqQueryable<CustomerDetails>(true).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            if (customer != null)
            {
                customer.Counter = customer.ServiceType == ServiceType.BankTransaction ? 1 : 2;
                customer.Status = status;
            }

            await this._container.UpsertItemAsync<CustomerDetails>(customer, new PartitionKey(id));
        }

        public IEnumerable<BankTokenDashboard> GetBankStaffTokenDetails()
        {
            return this._container.GetItemLinqQueryable<BankTokenDashboard>(true).AsEnumerable().ToList();
        }

        public IEnumerable<CustomerTokenDashboard> GetCustomerTokenDetails()
        {
            var customerList = this._container.GetItemLinqQueryable<CustomerDetails>(true).Where(x=> x.Status != Status.Served).AsEnumerable().ToList();

            List<CustomerTokenDashboard> customerTokenDetailsList = new List<CustomerTokenDashboard>();

            int bankTransactionQueue = 0;
            int serviceQueue = 0;
            foreach (var item in customerList)
            {
                var customerTokenDetails = new CustomerTokenDashboard {
                    Counter = item.Counter,
                    TokenNumber = item.TokenNumber,
                    ServiceType = item.ServiceType.ToString()
                };

                if (item.Status == Status.InQueue)
                {
                    if (item.ServiceType == ServiceType.BankTransaction)
                    {
                        ++bankTransactionQueue;
                        customerTokenDetails.EstimatedWaitingTime = 5 * bankTransactionQueue;

                    }

                    if (item.ServiceType == ServiceType.Service)
                    {
                        ++serviceQueue;
                        customerTokenDetails.EstimatedWaitingTime = 25 * serviceQueue;

                    }
                }

                customerTokenDetailsList.Add(customerTokenDetails);
            }

            return customerTokenDetailsList;
        }
    }
}
