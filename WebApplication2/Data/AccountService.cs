using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace WebApplication2.Data {
    public class AccountService : IAccountService {

        private Func<IDbConnection> Factory { get; }
        public AccountService(Func<IDbConnection> factory) {
            this.Factory = factory;
        }

        public async Task<List<Account>> GetAccountsAsync() {
            return await Task.Run(() => {
                using IDbConnection connection = this.Factory.Invoke();
                using IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Accounts";
                var reader = command.ExecuteReader();

                List<Account> accounts = new List<Account>();
                while (reader.Read()) {
                    accounts.Add(new Account {
                        Id = (int)reader["Id"],
                        AccountNo = (string)reader["AccountNo"],
                        Name = (string)reader["Name"]
                    });
                }

                return accounts;
            });
        }
    }
}
