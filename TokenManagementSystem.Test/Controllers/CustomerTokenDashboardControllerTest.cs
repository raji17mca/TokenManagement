using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            mockService.Setup(x => x.GetCustomerTokenDashboardDetails()).Returns(customerTokenDetailsList);

            // Act
            var actual = controller.Get();

            // Asset
            Assert.NotNull(actual);
            Assert.AreEqual(customerTokenDetailsList.Count, actual.Count());
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenFormIsInValid()
        {
            var mockService = new Mock<ITokenCosmosDBService>();

            mockService.Setup(x => x.AddCustomerDetails(It.IsAny<CustomerDetails>())).Returns(Task.FromResult(1));

            var controller = new CustomerTokenDashboardController(mockService.Object);
            controller.ModelState.AddModelError("ServiceType", "Required");

            var customerDetails = new CustomerDetails
            {
                Name = "Raji"
            };

            // Act
            var actual = await controller.Post(customerDetails) as ObjectResult;

            // Asset
            Assert.AreEqual(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsCreatedResult_WhenFormIsValid()
        {
            var mockService = new Mock<ITokenCosmosDBService>();

            mockService.Setup(x => x.AddCustomerDetails(It.IsAny<CustomerDetails>())).Returns(Task.FromResult(1));

            var controller = new CustomerTokenDashboardController(mockService.Object);

            var customerDetails = new CustomerDetails
            {
                Name = "Raji",
                AccountNumber = 123456,
                Age = 30,
                CustomerType ="Guest",
                ServiceType = ServiceType.BankTransaction,
                SocialNumber = "111111"
            };

            // Act
            var actual = await controller.Post(customerDetails) as ObjectResult;

            // Asset
            Assert.AreEqual(StatusCodes.Status201Created, actual.StatusCode);
            Assert.AreEqual(1, actual.Value);
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
