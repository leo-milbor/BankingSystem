using System.Diagnostics.CodeAnalysis;

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
            : this(new HashSet<Account>(new AccountComparer())) { }


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

    internal class AccountComparer : IEqualityComparer<Account>
    {
        public bool Equals(Account? x, Account? y)
        {
            if (x == null && y == null) return false;
            if (x == null || y == null) return false;
            return x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Account obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
