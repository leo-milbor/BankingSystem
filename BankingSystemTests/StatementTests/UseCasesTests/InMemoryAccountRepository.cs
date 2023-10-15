using BankingSystem.Statement;
using BankingSystem.Statement.UseCases;

namespace BankingSystemTests.StatementTests.UseCasesTests
{
    internal record InMemoryAccountRepository(Account? Account) : IAccountRepository
    {
        public Account? Get(string Id)
        {
            return Account;
        }
    }
}
