using BankingSystem.Account;
using BankingSystem.Account.UseCases;

namespace BankingSystemTests.AccountTests.UseCasesTests
{
    internal class InMemoryAccountRepository : IAccounRepository
    {
        private readonly ISet<Account> _accounts;

        public InMemoryAccountRepository(ISet<Account> accounts)
        {
            _accounts = accounts;
        }
        public InMemoryAccountRepository() : this(new HashSet<Account>()) { }

        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Name == account);
        }

        public ISet<Account> GetAll()
        {
            return new HashSet<Account>(_accounts);
        }

        public void Update(Account account)
        {
            // account is already updated for an in memory database.
        }
    }
}
