using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManagementSystem.Models;

namespace TokenManagementSystem.Services
{
    public interface ITokenCosmosDBService
    {
        Task<IEnumerable<TokenDetails>> GetItemsAsync(string query);

        Task<TokenDetails> GetItemAsync(string id);

        Task AddItemAsync(TokenDetails tokenDetails);

        Task UpdateItemAsync(string id, TokenDetails tokenDetails);

        Task DeleteItemAsync(string id);
    }
}
