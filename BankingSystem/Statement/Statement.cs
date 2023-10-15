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
            var orderedTransactions = account.Transactions.Where(t => t.Date >= date).OrderBy(t => t.Date);
            var orderedRules = rules.Where(r => r.Date < date.AddMonths(1)).OrderBy(r => r.Date);
            _transactions = new List<Transaction>();
            _transactions.AddRange(orderedTransactions);

            decimal interest = 0;
            foreach (var transaction in orderedTransactions)
            {
                foreach (var rule in orderedRules)
                {
                    var transactionDate = transaction.Date;
                    var balance = transaction.Balance;
                    var daysOfInterests = DaysOfInterests(transaction, orderedTransactions, rule, orderedRules);
                    interest += Math.Round(daysOfInterests * balance * (rule.Rate / 100) / 365, 2);
                }
            }

            var lastBalance = orderedTransactions.Last().Balance;
            _transactions.Add(NewInterestTransaction(date, lastBalance, interest));
        }

        private static int DaysOfInterests(Transaction transaction, IOrderedEnumerable<Transaction> transactions, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            int daysToNextElement = Math.Min(DaysToNextTransaction(transaction, transactions), DaysToNextRule(transaction, currentRule, rules));
            var latestDate = transaction.Date > currentRule.Date ? transaction.Date : currentRule.Date;
            return Math.Min(DaysToEndOfMonth(latestDate), daysToNextElement);
        }

        private static int DaysToNextRule(Transaction transaction, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            foreach (var rule in rules.Where(r => r.Date > currentRule.Date))
            {
                return rule.Date.Day - Math.Max(currentRule.Date.Day, transaction.Date.Day);
            }
            // If no next rule, let’s consider it too far in the future instead to ease calculus.
            return int.MaxValue;
        }

        private static int DaysToNextTransaction(Transaction transaction, IOrderedEnumerable<Transaction> dateOrderedTransactions)
        {
            foreach (var t in dateOrderedTransactions
                .Where(t => t.Id != transaction.Id && t.Date >= transaction.Date))
            {
                return t.Date.Day - transaction.Date.Day;
            }
            // If no next transaction, let’s consider it too far in the future instead to ease calculus.
            return int.MaxValue;
        }

        private static int DaysToEndOfMonth(DateOnly date)
        {
            return EndOfMonth(date).Day - date.Day;
        }

        private static DateOnly EndOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static Transaction NewInterestTransaction(DateOnly dateOfRule, decimal previousBalance, decimal interest)
        {
            return new Transaction("", EndOfMonth(dateOfRule), "I", interest, previousBalance + interest);
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

    internal record Transaction(string Id, DateOnly Date, string Type, decimal Amount, decimal Balance);

    internal record InterestRule(DateOnly Date, decimal Rate);
}
