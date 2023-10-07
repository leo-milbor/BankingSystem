using BankingSystem.Account.UseCases;

namespace BankingSystem.Account.Repository
{
    internal class InMemory : IAccounRepository
    {
        private readonly ISet<Account> _accounts;

        public InMemory(ISet<Account> accounts)
        {
            _accounts = accounts;
        }

        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Equals(account));
        }

        public void Update(Account account)
        {
            // account is already updated for an in memory DB.
        }
    }
}
