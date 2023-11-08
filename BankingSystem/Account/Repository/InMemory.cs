using BankingSystem.Account.UseCases;

namespace BankingSystem.Account.Repository
{
    internal class InMemory : IAccountRepository
    {
        private readonly ISet<Account> _accounts;

        private InMemory(ISet<Account> accounts)
        {
            _accounts = accounts;
        }
        public InMemory()
            : this(new HashSet<Account>()) { }


        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Account? Get(string account)
        {
            return _accounts.FirstOrDefault(a => a.Id == account);
        }

        public void Update(Account account)
        {
            var actual = Get(account.Id);
            if (actual is null) return;
            _accounts.Remove(actual);
            _accounts.Add(account);
        }
    }
}
