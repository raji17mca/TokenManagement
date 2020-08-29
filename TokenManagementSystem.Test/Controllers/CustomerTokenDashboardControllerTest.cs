using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TokenManagementSystem.Constants;
using TokenManagementSystem.Controllers;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Test.Controllers
{
    public class CustomerTokenDashboardControllerTest
    {
        [Test]
        public void Get_ReturnsBankTokenDashboardResultList_AsExpected()
        {
            // Arrange
            var mockService = new Mock<ITokenCosmosDBService>();
            var controller = new CustomerTokenDashboardController(mockService.Object);

            List<CustomerTokenDashboard> customerTokenDetailsList = GetCustomerTokenDetailsList();

            mockService.Setup(x => x.GetCustomerTokenDetails()).Returns(customerTokenDetailsList);

            // Act
            var actual = controller.Get();

            // Asset
            Assert.NotNull(actual);
            Assert.AreEqual(customerTokenDetailsList.Count, actual.Count());
        }


        private static List<CustomerTokenDashboard> GetCustomerTokenDetailsList()
        {
            return new List<CustomerTokenDashboard>
            {
                new CustomerTokenDashboard
                {
                    Counter = 1,
                    EstimatedWaitingTime = 25,
                    ServiceType = ServiceType.BankTransaction,
                    TokenNumber = 10
                },
                new CustomerTokenDashboard
                {
                    Counter = 2,
                    EstimatedWaitingTime = 5,
                    ServiceType = ServiceType.Service,
                    TokenNumber = 11
                }
            };
        }
    }
}
