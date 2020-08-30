namespace TokenManagementSystem.Services
{
    using Microsoft.Azure.Cosmos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TokenManagementSystem.Constants;
    using TokenManagementSystem.Models;

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

        public async Task<int> AddCustomerDetails(CustomerDetails customer)
        {
           var count = this._container.GetItemLinqQueryable<CustomerDetails>(true).AsQueryable().Count();
           
            customer.Id = Guid.NewGuid().ToString();
            customer.TokenNumber = ++count;
            customer.Status = Status.InQueue;

            await this._container.CreateItemAsync<CustomerDetails>(customer, new PartitionKey(customer.ServiceType));
            return customer.TokenNumber;
        }


        public async Task<bool> UpdateCustomerDetails(string id, string status)
        {
            var customer = this._container.GetItemLinqQueryable<CustomerDetails>(true).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            if(customer != null)
            {
                customer.Counter = customer.ServiceType == ServiceType.BankTransaction ? 1 : 2;
                customer.Status = status;
                await this._container.UpsertItemAsync<CustomerDetails>(customer, new PartitionKey(customer.ServiceType));
                return true;
            }

            return false;
           
        }

        public IList<BankTokenDashboard> GetBankTokenDashboardDetails()
        {
            return this._container.GetItemLinqQueryable<BankTokenDashboard>(true).AsQueryable().ToList();
        }

        public IList<CustomerTokenDashboard> GetCustomerTokenDashboardDetails()
        {
            var customerList = this._container.GetItemLinqQueryable<CustomerDetails>(true).Where(x=> x.Status != Status.Served).AsQueryable().ToList();

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
