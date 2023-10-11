namespace BankingSystem.Statement
{
    internal class Statement
    {
        public void AccountStatement(Account account, IEnumerable<InterestRule> rules, DateOnly date)
        {
            var balanceAtDate = account.Transactions.Where(t => t.Date < date).Sum(t => t.Amount);
            var statementTransactions = account.Transactions.Where(t => t.Date >= date);
            var firstTransaction = statementTransactions.OrderBy(t => t.Date).First();
            firstTransaction = new Transaction(
                firstTransaction.Id, firstTransaction.Date, firstTransaction.Type,
                firstTransaction.Balance, firstTransaction.Amount);
            var transactions = new List<Transaction>(statementTransactions.Count())
            { firstTransaction};
            transactions.AddRange(statementTransactions.Where(t => t.Date == firstTransaction.Date));

            var maxTransactionDate = transactions.Max(t => t.Date);
            var rulesAtDate = rules.Where(r => r.Date >= date && r.Date <= maxTransactionDate).ToList();

            var result = new List<Transaction>(transactions.Count + rulesAtDate.Count);
            decimal interest = 0;
            for (int i = 0; i < rulesAtDate.Count; i++)
            {
                var rule = rulesAtDate[i];
                var nextRule = rulesAtDate[i + 1];
                var transactionsAtDate = transactions.Where(t => rule.Date <= t.Date && t.Date < nextRule.Date);
                var lastTransaction = transactionsAtDate.OrderBy(t => t.Date).Last();
                var nbDay = Math.Max(nextRule.Date.Day, lastTransaction.Date.Day) - rule.Date.Day;
                interest += transactions.Sum(t => t.Balance) * rule.Rate * nbDay / 365;
                result.AddRange(transactionsAtDate);
                var oninterestTransaction = new Transaction(
                    "", new DateOnly(rule.Date.Year, rule.Date.Month, rule.Date.AddDays(-rule.Date.Day).AddMonths(1).Day),
                    "I", lastTransaction.Balance + interest, interest);
                result.Add(oninterestTransaction);
            }
        }
    }
    internal class Account
    {
        public IEnumerable<Transaction> Transactions { get; }
    }
    internal class Transaction
    {
        public Transaction(string id, DateOnly date, string type, decimal balance, decimal amount)
        {
            Id = id;
            Date = date;
            Type = type;
            Balance = balance;
            Amount = amount;
        }

        public string Id { get; }
        public DateOnly Date { get; }
        public string Type { get; }
        public decimal Balance { get; }
        public decimal Amount{ get; }
    }
    internal class InterestRule
    {
        public DateOnly Date { get; }
        public decimal Rate { get; }
    }
}
