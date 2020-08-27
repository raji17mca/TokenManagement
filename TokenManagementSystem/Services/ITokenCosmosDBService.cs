using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManagementSystem.Models;

namespace TokenManagementSystem.Services
{
    public interface ITokenCosmosDBService
    {
        IEnumerable<CustomerTokenDetails> GetCustomerTokenDetails(string query);

        IEnumerable<BankStaffTokenDetails> GetBankStaffTokenDetails(string query);

        Task AddItemAsync(CustomerDetails customer);

        void UpdateItemAsync(string id, CustomerDetails customerDetails);
    }
}
