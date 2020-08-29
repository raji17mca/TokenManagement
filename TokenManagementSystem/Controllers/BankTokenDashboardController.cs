using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Controllers
{
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
        public IEnumerable<BankTokenDashboard> Get()
        {
            return this.tokenCosmosDbService.GetBankStaffTokenDetails();
        }

        // PUT api/<BankTokenDashboardController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                var result = await this.tokenCosmosDbService.UpdateItemAsync(id, status);

                return result == true ? Ok() : (IActionResult)NotFound();
            }

            return BadRequest("Not a valid status");
        }
    }
}
