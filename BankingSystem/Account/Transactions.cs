using System.Text;

namespace BankingSystem.Account
{
    internal class Transactions
    {
        private readonly List<Transaction> _transactions;

        public void Add(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public Transactions(IEnumerable<Transaction> transactions)
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var transaction in _transactions.Select(t => t.ToString()))
                sb.AppendLine(transaction);

            return sb.ToString();
        }
    }
}
