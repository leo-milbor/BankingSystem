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
        public InMemory() : this(new HashSet<Account>()) { }


        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Name == account);
        }

        public void Update(Account account)
        {
            // account is already updated for an in memory database.
        }
    }
}
