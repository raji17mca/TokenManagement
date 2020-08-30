namespace TokenManagementSystem.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TokenManagementSystem.Constants;
    using TokenManagementSystem.Models;
    using TokenManagementSystem.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class BankTokenDashboardController : ControllerBase
    {
        private readonly ITokenCosmosDBService tokenCosmosDbService;
        public BankTokenDashboardController(ITokenCosmosDBService tokenCosmosDbService)
        {
            this.tokenCosmosDbService = tokenCosmosDbService;
        }

        // GET api/<BankTokenDashboardController>/Get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<BankTokenDashboard> Get()
        {
            return this.tokenCosmosDbService.GetBankTokenDashboardDetails();
        }

        // PUT api/<BankTokenDashboardController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(string id, [FromBody] string status)
        {
            if (!string.IsNullOrEmpty(status) && (Status.InCounter == status || Status.Served == status))
            {
                var result = await this.tokenCosmosDbService.UpdateCustomerDetails(id, status);

                return result == true ? Ok() : (IActionResult)NotFound();
            }

            return BadRequest("Not a valid status");
        }
    }
}
