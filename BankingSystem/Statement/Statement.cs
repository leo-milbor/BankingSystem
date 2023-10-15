using BankingSystem.Account;

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
            var transactionsOfMonth = account.Transactions.Where(t => t.Date >= date && t.Date.Month == date.Month);
            var orderedRules = rules.Where(r => r.Date < date.AddMonths(1)).OrderBy(r => r.Date);
            _transactions = new List<Transaction>();
            _transactions.AddRange(transactionsOfMonth);

            decimal interest = 0;
            var transactionsForInterestCalculus = transactionsOfMonth
                .Where(t => IsLastTransactionOfDay(t, transactionsOfMonth))
                .OrderBy(t => t.Date);
            foreach (var transaction in transactionsForInterestCalculus)
            {
                var firstApplicableRule = orderedRules.Last(r => r.Date <= transaction.Date);
                var applicableRules = new List<InterestRule>() { firstApplicableRule };
                var restOfApplicableRules = orderedRules
                    .Where(r => r.Date > firstApplicableRule.Date
                             && r.Date.Day < DaysToNextTransaction(transaction, transactionsForInterestCalculus));
                applicableRules.AddRange(restOfApplicableRules);
                foreach (var rule in applicableRules.OrderBy(r => r.Date))
                {
                    var transactionDate = transaction.Date;
                    var balance = transaction.Balance;
                    var daysOfInterests = DaysOfInterests(transaction, transactionsForInterestCalculus, rule, orderedRules);
                    interest += Math.Round(daysOfInterests * balance * (rule.Rate / 100) / 365, 2);
                }
            }

            var lastBalance = transactionsForInterestCalculus.Last().Balance;
            _transactions.Add(NewInterestTransaction(date, lastBalance, interest));
        }

        private static bool IsLastTransactionOfDay(Transaction transaction, IEnumerable<Transaction> transactions)
        {
            return !transactions
                .Any(t => t.Date == transaction.Date
                       && t.RunningNumber > transaction.RunningNumber);
        }

        private static int DaysOfInterests(Transaction transaction, IOrderedEnumerable<Transaction> transactions, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            var daysToNextElement = DaysToNextElement(transaction, transactions, currentRule, rules);
            var latestDate = transaction.Date > currentRule.Date ? transaction.Date : currentRule.Date;
            return Math.Min(DaysToEndOfMonth(latestDate), daysToNextElement);
        }

        private static int DaysToNextElement(Transaction transaction, IOrderedEnumerable<Transaction> transactions, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            return Math.Min(DaysToNextTransaction(transaction, transactions), DaysToNextRule(transaction, currentRule, rules));
        }

        private static int DaysToNextRule(Transaction transaction, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            var lastCalculusDateDays = currentRule.Date.Month < transaction.Date.Month
                ? transaction.Date.Day
                : Math.Max(currentRule.Date.Day, transaction.Date.Day);
            foreach (var rule in rules.Where(r => r.Date > currentRule.Date))
            {
                return rule.Date.Day - lastCalculusDateDays;
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
            return new Transaction("", EndOfMonth(dateOfRule), 1, "I", interest, previousBalance + interest);
        }
    }
    internal record Account(string Id, IEnumerable<Transaction> Transactions);

    internal record Transaction(string Id, DateOnly Date, int RunningNumber, string Type, decimal Amount, decimal Balance);

    internal record InterestRule(DateOnly Date, decimal Rate);
}
