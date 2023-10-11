namespace BankingSystem.Account
{
    internal class Account
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

        internal class EmptyTransactionsException : Exception { }
        internal class NegativeBalanceException : Exception { }
    }
}
