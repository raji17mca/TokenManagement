using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManagementSystem.Constants;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Test.Services
{
    public class TokenCosmosDBServiceTest
    {
        [Test]
        public async Task AddCustomerDetails_ShouldAddCustomerDetails_ReturnTokenNumber()
        {
            var mockContainer = new Mock<Container>();
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockQueryable = new Mock<IOrderedQueryable>();

            var queyableCustomerDetails = new List<CustomerDetails>{
                new CustomerDetails { Name = "Raji"}
            }.AsQueryable();
      

            mockContainer.Setup(x => x.GetItemLinqQueryable<CustomerDetails>(true, null, null)).Returns((IOrderedQueryable<CustomerDetails>)queyableCustomerDetails);
            mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(mockContainer.Object);
            var service = new TokenCosmosDbService(mockCosmosClient.Object, "CosmosDb", "Collection");

            // Act
            var actual = await service.AddCustomerDetails(new CustomerDetails());

            // Assert
            Assert.AreEqual(2, actual);
        }

        [Test]
        public async Task UpdateCustomerDetails_ShouldUpdateCustomerDetails_ReturnTrue()
        {
            // Arrange
            var queyableCustomerDetails = new List<CustomerDetails>{
                new CustomerDetails { Id ="12345", Name = "Raji"}
            }.AsQueryable();


            Mock<CosmosClient> mockCosmosClient = MockCosmosClient(queyableCustomerDetails);

            var service = new TokenCosmosDbService(mockCosmosClient.Object, "CosmosDb", "Collection");

            // Act
            var actual = await service.UpdateCustomerDetails("12345", Status.Served);

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public async Task UpdateCustomerDetails_ShouldNotUpdateCustomerDetails_ReturnFalse()
        {
            // Arrange 
            var queyableCustomerDetails = new List<CustomerDetails>{
                new CustomerDetails { Id ="12345", Name = "Raji"}
            }.AsQueryable();

            Mock<CosmosClient> mockCosmosClient = MockCosmosClient(queyableCustomerDetails);

            var service = new TokenCosmosDbService(mockCosmosClient.Object, "CosmosDb", "Collection");

            // Act
            var actual = await service.UpdateCustomerDetails("111111", Status.Served);

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void GetBankTokenDashboardDetails__ReturnResultsAsExpected()
        {
            var mockContainer = new Mock<Container>();
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockQueryable = new Mock<IOrderedQueryable>();

            var queryableResult = new List<BankTokenDashboard>{
                new BankTokenDashboard { Id ="12345", ServiceType = ServiceType.Service, TokenNumber = 1, Status = Status.InQueue },
                new BankTokenDashboard { Id ="55555", ServiceType = ServiceType.BankTransaction, TokenNumber = 2, Status = Status.InCounter }
            }.AsQueryable();


            mockContainer.Setup(x => x.GetItemLinqQueryable<BankTokenDashboard>(true, null, null)).Returns((IOrderedQueryable<BankTokenDashboard>)queryableResult);
            mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(mockContainer.Object);

            var service = new TokenCosmosDbService(mockCosmosClient.Object, "CosmosDb", "Collection");

            // Act
            var actual = service.GetBankTokenDashboardDetails();

            // Assert
           
            Assert.AreEqual(queryableResult.Count(), actual.Count());
        }

        [Test]
        public void GetCustomerDashboardTokenDetails__ReturnEstimatedWaitingTImeAsExpected()
        {
            var queyableCustomerDetails = new List<CustomerDetails>{
                new CustomerDetails { Id ="12345", ServiceType = ServiceType.Service, TokenNumber = 1, Status = Status.Served },
                new CustomerDetails { Id ="55555", ServiceType = ServiceType.Service, Counter = 1,TokenNumber = 2, Status = Status.InQueue },
                new CustomerDetails { Id ="66666", ServiceType = ServiceType.BankTransaction, Counter = 2, TokenNumber = 3, Status = Status.InQueue }
            }.AsQueryable();

            Mock<CosmosClient> mockCosmosClient = MockCosmosClient(queyableCustomerDetails);

            var service = new TokenCosmosDbService(mockCosmosClient.Object, "CosmosDb", "Collection");

            // Act
            var actual = service.GetCustomerTokenDashboardDetails();

            // Assert

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(25, actual[0].EstimatedWaitingTime);
            Assert.AreEqual(5, actual[1].EstimatedWaitingTime);
        }

        private static Mock<CosmosClient> MockCosmosClient(IQueryable<CustomerDetails> queyableCustomerDetails)
        {
            var mockContainer = new Mock<Container>();
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockQueryable = new Mock<IOrderedQueryable>();

            mockContainer.Setup(x => x.GetItemLinqQueryable<CustomerDetails>(true, null, null)).Returns((IOrderedQueryable<CustomerDetails>)queyableCustomerDetails);
            mockCosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(mockContainer.Object);
            return mockCosmosClient;
        }
    }
}
