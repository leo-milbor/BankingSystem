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

        public void Add(BS.Account account)
        {
            _accounts.Add(account);
        }

        public BS.Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Equals(account));
        }

        public void Update(BS.Account account)
        {
            // account is already updated for an in memory database.
        }
    }
}
