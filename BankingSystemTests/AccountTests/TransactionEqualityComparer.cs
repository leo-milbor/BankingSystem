using System.Diagnostics.CodeAnalysis;

using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    internal class TransactionEqualityComparer : IEqualityComparer<Transaction>
    {
        public bool Equals(Transaction? x, Transaction? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            return x.Date.Value == y.Date.Value && x.RunningNumber == y.RunningNumber && x.Type == y.Type && x.Amount == y.Amount;
        }

        public int GetHashCode([DisallowNull] Transaction obj)
        {
            return HashCode.Combine(obj.RunningNumber, obj.Date, obj.Type, obj.Amount);
        }
    }

    internal class TransactionsEqualityComparer : IEqualityComparer<IEnumerable<Transaction>>
    {
        public bool Equals(IEnumerable<Transaction>? x, IEnumerable<Transaction>? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            return x.SequenceEqual(y, new TransactionEqualityComparer());
        }

        public int GetHashCode([DisallowNull] IEnumerable<Transaction> obj)
        {
            return HashCode.Combine(obj.ToArray());
        }
    }
}
