using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TokenManagementSystem.Models;
using TokenManagementSystem.Services;

namespace TokenManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenCosmosDBService tokenCosmosDbService;
        public TokenController(ITokenCosmosDBService tokenCosmosDbService)
        {
            this.tokenCosmosDbService = tokenCosmosDbService;
        }

        // GET: api/<TokenController>
        [HttpGet]
        public IEnumerable<CustomerTokenDetails> Get()
        {
            return  this.tokenCosmosDbService.GetCustomerTokenDetails("SELECT * FROM C");
        }

        // GET api/<TokenController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TokenController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDetails customerDetails)
        {
            if (ModelState.IsValid)
            {
               customerDetails.Id = Guid.NewGuid().ToString();
               await this.tokenCosmosDbService.AddItemAsync(customerDetails);
                return Ok();
            }

            return BadRequest("Not a valid model");
        }

        // PUT api/<TokenController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
