using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManagementSystem.Models;

namespace TokenManagementSystem.Services
{
    public interface ITokenCosmosDBService
    {
        IEnumerable<CustomerTokenDashboard> GetCustomerTokenDetails();

        IEnumerable<BankTokenDashboard> GetBankStaffTokenDetails();

        Task<int> AddItemAsync(CustomerDetails customer);

        Task<bool> UpdateItemAsync(string id, string status);
    }
}
