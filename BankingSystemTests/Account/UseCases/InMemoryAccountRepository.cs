using BankingSystem.Account.UseCases;

using BS = BankingSystem.Account;

namespace BankingSystemTests.Account.Repository
{
    internal class InMemoryAccountRepository : IAccounRepository
    {
        private readonly ISet<BS.Account> _accounts;

        public InMemoryAccountRepository(ISet<BS.Account> accounts)
        {
            _accounts = accounts;
        }
        public InMemoryAccountRepository() : this(new HashSet<BS.Account>()) { }

        public void Add(BS.Account account)
        {
            _accounts.Add(account);
        }

        public BS.Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Name == account);
        }

        public ISet<BS.Account> GetAll()
        {
            return new HashSet<BS.Account>(_accounts);
        }

        public void Update(BS.Account account)
        {
            // account is already updated for an in memory database.
        }
    }
}
