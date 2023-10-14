namespace BankingSystem.Statement
{
    internal class Statement
    {
        private readonly List<Transaction> _transactions;

        public string Id { get; }
        public IEnumerable<Transaction> Transactions => _transactions;
        public Statement(Account account, IEnumerable<InterestRule> rules, DateOnly date)
        {
            Id = account.Id;
            _transactions = new List<Transaction>();
            _transactions.AddRange(account.Transactions);

            var transaction = _transactions.First();
            var transactionDate = transaction.Date;
            var balance = transaction.Balance;
            var interest = daysToEndOfMonth(transactionDate) * balance * (rules.First().Rate / 100) / 365;

            _transactions.Add(NewInterestTransaction(date, balance, interest));
        }

        private static int daysToEndOfMonth(DateOnly date)
        {
            return EndOfMonth(date).Day - date.Day;
        }

        private static DateOnly EndOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static Transaction NewInterestTransaction(DateOnly dateOfRule, decimal previousBalance, decimal interest)
        {
            return new Transaction("", EndOfMonth(dateOfRule), "I", previousBalance + interest, interest);
        }

        //private void calc()
        //{
        //    var balanceAtDate = account.Transactions.Where(t => t.Date < date).Sum(t => t.Amount);
        //    var statementTransactions = account.Transactions.Where(t => t.Date >= date);
        //    var firstTransaction = statementTransactions.OrderBy(t => t.Date).First();
        //    firstTransaction = new Transaction(
        //        firstTransaction.Id, firstTransaction.Date, firstTransaction.Type,
        //        firstTransaction.Balance, firstTransaction.Amount);
        //    var transactions = new List<Transaction>(statementTransactions.Count())
        //    { firstTransaction};
        //    transactions.AddRange(statementTransactions.Where(t => t.Date == firstTransaction.Date));

        //    var maxTransactionDate = transactions.Max(t => t.Date);
        //    var rulesAtDate = rules.Where(r => r.Date >= date && r.Date <= maxTransactionDate).ToList();

        //    var result = new List<Transaction>(transactions.Count + rulesAtDate.Count);
        //    decimal interest = 0;
        //    for (int i = 0; i < rulesAtDate.Count; i++)
        //    {
        //        var rule = rulesAtDate[i];
        //        var nextRule = i + 1 != rulesAtDate.Count ?
        //            rulesAtDate[i + 1] : rule;
        //        var transactionsAtDate = transactions.Where(t => rule.Date <= t.Date && t.Date < nextRule.Date);
        //        var lastTransaction = transactionsAtDate.OrderBy(t => t.Date).Last();
        //        var nbDay = Math.Max(nextRule.Date.Day, lastTransaction.Date.Day) - rule.Date.Day;
        //        interest += transactions.Sum(t => t.Balance) * (rule.Rate / 100) * nbDay / 365;
        //        result.AddRange(transactionsAtDate);
        //        var interestTransaction = new Transaction(
        //            "", new DateOnly(rule.Date.Year, rule.Date.Month, rule.Date.AddDays(-rule.Date.Day).AddMonths(1).Day),
        //            "I", lastTransaction.Balance + interest, interest);
        //        result.Add(interestTransaction);
        //    }
        //    Transactions = result;
        //}
    }
    internal record Account(string Id, IEnumerable<Transaction> Transactions);

    internal record Transaction(string Id, DateOnly Date, string Type, decimal Balance, decimal Amount);

    internal record InterestRule(DateOnly Date, decimal Rate);
}
