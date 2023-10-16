using System.Transactions;

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

            decimal anualizedInterest = 0;
            var transactionsForInterestCalculus = transactionsOfMonth
                .Where(t => IsLastTransactionOfDay(t, transactionsOfMonth))
                .OrderBy(t => t.Date);
            foreach (var transaction in transactionsForInterestCalculus)
            {
                var applicableRules = GetApplicableRules(transaction, transactionsForInterestCalculus, orderedRules);
                foreach (var rule in applicableRules.OrderBy(r => r.Date))
                {
                    anualizedInterest += GetAnnualizedInterest(transaction, rule, transactionsForInterestCalculus, orderedRules);
                }
            }

            var lastBalance = transactionsForInterestCalculus.Last().Balance;
            _transactions.Add(NewInterestTransaction(date, lastBalance, Math.Round(anualizedInterest / 365, 2)));
        }

        private static decimal GetAnnualizedInterest(Transaction transaction, InterestRule rule, IOrderedEnumerable<Transaction> transactions, IOrderedEnumerable<InterestRule> rules)
        {
            var balance = transaction.Balance;
            var daysOfInterests = DaysOfInterests(transaction, transactions, rule, rules);
            var interestForTransaction = Math.Round(daysOfInterests * balance * (rule.Rate / 100), 2);
            return interestForTransaction;
        }

        private static IEnumerable<InterestRule> GetApplicableRules(Transaction transaction, IOrderedEnumerable<Transaction> transactionsForInterestCalculus, IOrderedEnumerable<InterestRule> orderedRules)
        {
            var firstApplicableRule = orderedRules.Last(r => r.Date <= transaction.Date);
            var applicableRules = new List<InterestRule>() { firstApplicableRule };
            var restOfApplicableRules = orderedRules
                .Where(r => r.Date > firstApplicableRule.Date
                         && r.Date.Day < DaysToNextTransaction(transaction, DateOnly.MinValue, transactionsForInterestCalculus));
            applicableRules.AddRange(restOfApplicableRules);
            return applicableRules;
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

            var firstTransactionIsAt1stOfMonth = transactions.First().Date.Day == 1;
            var endOfMonthOffset = firstTransactionIsAt1stOfMonth ? 1 : 0;
            var latestDate = transaction.Date > currentRule.Date ? transaction.Date : currentRule.Date;
            return Math.Min(DaysToEndOfMonth(latestDate, endOfMonthOffset), daysToNextElement);
        }

        private static int DaysToNextElement(Transaction transaction, IOrderedEnumerable<Transaction> transactions, InterestRule currentRule, IOrderedEnumerable<InterestRule> rules)
        {
            return Math.Min(DaysToNextTransaction(transaction, currentRule.Date, transactions), DaysToNextRule(transaction, currentRule, rules));
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

        private static int DaysToNextTransaction(Transaction transaction, DateOnly ruleDate, IOrderedEnumerable<Transaction> dateOrderedTransactions)
        {
            var currentPeriodStart = MaxDate(transaction.Date, ruleDate);
            foreach (var t in dateOrderedTransactions
                .Where(t => t.Id != transaction.Id && t.Date >= transaction.Date))
            {
                return t.Date.Day - currentPeriodStart.Day;
            }
            // If no next transaction, let’s consider it too far in the future instead to ease calculus.
            return int.MaxValue;
        }

        private static int DaysToEndOfMonth(DateOnly date, int endOfMonthOffset)
        {
            return EndOfMonth(date).Day - date.Day + endOfMonthOffset;
        }

        private static DateOnly EndOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static DateOnly MaxDate(DateOnly x, DateOnly y) => x > y ? x : y;

        private static Transaction NewInterestTransaction(DateOnly dateOfRule, decimal previousBalance, decimal interest)
        {
            return new Transaction("", EndOfMonth(dateOfRule), 1, "I", interest, previousBalance + interest);
        }
    }
    internal record Account(string Id, IEnumerable<Transaction> Transactions);

    internal record Transaction(string Id, DateOnly Date, int RunningNumber, string Type, decimal Amount, decimal Balance);

    internal record InterestRule(DateOnly Date, decimal Rate);
}
