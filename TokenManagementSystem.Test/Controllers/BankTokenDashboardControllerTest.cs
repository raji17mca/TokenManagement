using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManagementSystem.Constants;
using TokenManagementSystem.Controllers;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Test.Controllers
{
    public class BankTokenDashboardControllerTest
    {
        [Test]
        public void Get_ReturnsBankTokenDashboardResultList_AsExpected()
        {
            // Arrange
            var mockService = new Mock<ITokenCosmosDBService>();
            var controller = new BankTokenDashboardController(mockService.Object);

            List<BankTokenDashboard> banktokenDashboardList = GetBankTokenDashboardList();

            mockService.Setup(x => x.GetBankStaffTokenDetails()).Returns(banktokenDashboardList);

            // Act
            var actual = controller.Get();

            // Asset
            Assert.NotNull(actual);
            Assert.AreEqual(banktokenDashboardList.Count, actual.Count());
        }

        [Test]
        public async Task Put_ReturnsBadRequestResult_WhenStatusIsInValid()
        {
            var mockService = new Mock<ITokenCosmosDBService>();

            var controller = new BankTokenDashboardController(mockService.Object);

            // Act
            var actual = await controller.Put(Guid.NewGuid().ToString(), null) as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;

            // Asset
            Assert.AreEqual( StatusCodes.Status400BadRequest, actual.StatusCode);
            Assert.AreEqual("Not a valid status", actual.Value);
        }

        [Test]
        public async Task Put_ReturnsOkRequestResult_WhenStatusIsInValid()
        {
            var mockService = new Mock<ITokenCosmosDBService>();

            mockService.Setup(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            var controller = new BankTokenDashboardController(mockService.Object);

            // Act
            var actual = await controller.Put(Guid.NewGuid().ToString(), Status.Served) as Microsoft.AspNetCore.Mvc.OkResult;

            // Asset
            Assert.AreEqual(StatusCodes.Status200OK, actual.StatusCode);
            
        }

        [Test]
        public async Task Put_ReturnsNotFoundRequestResult_WhenIdIsNotExist()
        {
            var mockService = new Mock<ITokenCosmosDBService>();

            mockService.Setup(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            var controller = new BankTokenDashboardController(mockService.Object);

            // Act
            var actual = await controller.Put(Guid.NewGuid().ToString(), Status.Served) as Microsoft.AspNetCore.Mvc.NotFoundResult;

            // Asset
            Assert.AreEqual(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        private static List<BankTokenDashboard> GetBankTokenDashboardList()
        {
            return new List<BankTokenDashboard>
            {
                new BankTokenDashboard
                {
                    Id = Guid.NewGuid().ToString(),
                    ServiceType = ServiceType.BankTransaction,
                    Status = Status.InCounter,
                    TokenNumber = 1
                },
                 new BankTokenDashboard
                {
                    Id = Guid.NewGuid().ToString(),
                    ServiceType = ServiceType.BankTransaction,
                    Status = Status.InCounter,
                    TokenNumber = 2
                }
            };
        }
    }
}
