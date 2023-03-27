using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.Data {
    public interface IAccountService {
        Task<List<Account>> GetAccountsAsync();
    }
}
