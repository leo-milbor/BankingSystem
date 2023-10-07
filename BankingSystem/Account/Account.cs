using System.Text;

namespace BankingSystem.Account
{
    internal class Account : IEquatable<Account?>
    {
        private readonly string _name;
        private readonly Transactions _transactions;

        public Account(string name, Transactions transactions)
        {
            _name = name;
            _transactions = transactions;
        }

        public void Add(Date date, TransactionType type, Amount amount)
        {
           var runningNumber = _transactions.AtDate(date).MaxRunningNumber();
            var transaction = new Transaction(runningNumber, date, type, amount);
            _transactions.Add(transaction);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_name);
            sb.Append(_transactions.ToString());
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Account);
        }

        public bool Equals(Account? other)
        {
            return other is not null &&
                   _name == other._name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name);
        }

        public static bool operator ==(Account? left, Account? right)
        {
            return EqualityComparer<Account>.Default.Equals(left, right);
        }

        public static bool operator !=(Account? left, Account? right)
        {
            return !(left == right);
        }
    }
}
