using System.Text;

namespace BankingSystem.Account
{
    internal class Account : IEquatable<Account?>
    {
        private readonly string _name;
        private readonly Transactions _transactions;

        public string Name => _name;
        public IEnumerable<Transaction> Transactions => _transactions.Value;

        public Account(string name, Transactions transactions)
        {
            if (transactions.Empty)
                throw new EmptyTransactionsException();
            _name = name;
            _transactions = transactions;
        }

        public Account(string name, Transaction transaction)
        {
            if (transaction.IsWithdrawal)
                throw new NegativeBalanceException();
            _name = name;
            _transactions = new Transactions(transaction);
        }

        public void AddTransaction(Date date, TransactionType type, Amount amount)
        {
            var runningNumber = NextRunningNumber(date);
            var transaction = new Transaction(runningNumber, date, type, amount);
            _transactions.Add(transaction);
            if (_transactions.HasNegtiveBalance)
                throw new NegativeBalanceException();
        }

        private int NextRunningNumber(Date date)
        {
            return _transactions.AtDate(date).MaxRunningNumber() + 1;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Account: {_name}").AppendLine();
            sb.Append(_transactions.ToString());
            return sb.ToString();
        }

        #region Equality
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
        #endregion Equality

        internal class EmptyTransactionsException : Exception { }
        internal class NegativeBalanceException : Exception { }
    }
}
