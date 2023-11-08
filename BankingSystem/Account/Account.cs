namespace BankingSystem.Account
{
    internal class Account
    {
        private readonly string _id;
        private readonly Transactions _transactions;

        public string Id => _id;
        public IEnumerable<Transaction> Transactions => _transactions.Value;

        public Account(string id, Transactions transactions)
        {
            if (transactions.Empty)
                throw new EmptyTransactionsException();
            _id = id;
            _transactions = transactions;
        }

        public Account(string id, Transaction transaction)
        {
            if (transaction.IsWithdrawal)
                throw new NegativeBalanceException();
            _id = id;
            _transactions = new Transactions(transaction);
        }

        public Account AddTransaction(Date date, TransactionType type, Amount amount)
        {
            var runningNumber = NextRunningNumber(date);
            var transaction = new Transaction(runningNumber, date, type, amount);
            var transactions = _transactions.Add(transaction);
            if (transactions.HasNegtiveBalance)
                throw new NegativeBalanceException();
            return new Account(_id, transactions);
        }

        private int NextRunningNumber(Date date)
        {
            return _transactions.AtDate(date).MaxRunningNumber() + 1;
        }

        internal class EmptyTransactionsException : Exception { }
        internal class NegativeBalanceException : Exception { }
    }
}
