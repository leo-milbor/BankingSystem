namespace BankingSystem.Account
{
    internal class Transactions
    {
        private readonly IList<Transaction> _transactions;

        public bool Empty => _transactions.Count == 0;
        public bool HasNegtiveBalance => _transactions
            .OrderBy(t => t.Date)
            .Sum(SumByAmount) < 0;
        public IEnumerable<Transaction> Value => new List<Transaction>(_transactions);

        public void Add(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        private Transactions(IEnumerable<Transaction> transactions)
        {
            _transactions = transactions.ToList();
        }

        public Transactions(Transaction transaction) : this(new List<Transaction>() { transaction })
        {
        }

        public Transactions AtDate(Date date)
        {
            return new Transactions(_transactions.Where(t => t.Date == date));
        }

        public int MaxRunningNumber()
        {
            var transaction = _transactions.MaxBy(t => t.RunningNumber);
            if (transaction is null)
            {
                return 0;
            }
            return transaction.RunningNumber;
        }

        private static decimal SumByAmount(Transaction transaction) => transaction.IsWithdrawal ? -transaction.Amount : transaction.Amount;
    }
}
