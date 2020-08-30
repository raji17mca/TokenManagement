namespace TokenManagementSystem.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TokenManagementSystem.Models;

    public interface ITokenCosmosDBService
    {
        IList<CustomerTokenDashboard> GetCustomerTokenDashboardDetails();

        IList<BankTokenDashboard> GetBankTokenDashboardDetails();

        Task<int> AddCustomerDetails(CustomerDetails customer);

        Task<bool> UpdateCustomerDetails(string id, string status);
    }
}
