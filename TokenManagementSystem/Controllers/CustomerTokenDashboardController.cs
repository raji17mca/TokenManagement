using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTokenDashboardController : ControllerBase
    {
        private readonly ITokenCosmosDBService tokenCosmosDbService;
        public CustomerTokenDashboardController(ITokenCosmosDBService tokenCosmosDbService)
        {
            this.tokenCosmosDbService = tokenCosmosDbService;
        }

        // GET: api/<CustomerTokenDashboardController>/Get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<CustomerTokenDashboard> Get()
        {
            return this.tokenCosmosDbService.GetCustomerTokenDashboardDetails();
        }

        // POST api/<CustomerTokenDashboardController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CustomerDetails customerDetails)
        {
            if (ModelState.IsValid)
            {
               var result = await this.tokenCosmosDbService.AddCustomerDetails(customerDetails);
               return StatusCode(StatusCodes.Status201Created, result);
            }

            return BadRequest("Not a valid model");
        }
    }
}
