using BankingSystem.Statement;

namespace BankingSystemTests.StatementTests
{
    public class StatementTests
    {
        [Fact]
        public void Statement_for_1_transaction_1_interest_rule()
        {
            decimal interest = Math.Round(100 * 0.1m * 21 / 365, 2);
            var expected = new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 100, 100),
                new Transaction("", new DateOnly(2023, 10, 31), "I", interest, 100 + interest)
            };
            var account = new Account("AC001", new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 100, 100)
            });
            var rule = new InterestRule(new DateOnly(2023, 10, 03), 10m);

            var statement = new Statement(account, new List<InterestRule>() { rule }, new DateOnly(2023, 10, 01));

            var statementTransactions = statement.Transactions;
            Assert.Equal("AC001", statement.Id);
            Assert.Equal(2, statementTransactions.Count());
            Assert.Equal(expected, statementTransactions.ToList());
        }

        [Fact]
        public void Statement_for_2_transactions_1_interest_rule()
        {
            decimal interest = Math.Round(50 * 0.1m * 5 / 365, 2) + Math.Round(100 * 0.1m * 16 / 365, 2);
            var expected = new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 50, 50),
                new Transaction("20231015-01", new DateOnly(2023, 10, 15), "D", 50, 100),
                new Transaction("", new DateOnly(2023, 10, 31), "I", interest, 100 + interest)
            };
            var account = new Account("AC001", new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 50, 50),
                new Transaction("20231015-01", new DateOnly(2023, 10, 15), "D", 50, 100),
            });
            var rule = new InterestRule(new DateOnly(2023, 10, 03), 10m);

            var statement = new Statement(account, new List<InterestRule>() { rule }, new DateOnly(2023, 10, 01));

            var statementTransactions = statement.Transactions;
            Assert.Equal("AC001", statement.Id);
            Assert.Equal(3, statementTransactions.Count());
            Assert.Equal(expected, statementTransactions.ToList());
        }

        [Fact]
        public void Statement_for_1_transactions_2_interest_rule()
        {
            decimal interest = Math.Round(100 * 0.1m * 5 / 365, 2) +  Math.Round(100 * 0.2m * 16 / 365, 2);
            var expected = new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 100, 100),
                new Transaction("", new DateOnly(2023, 10, 31), "I", interest, 100 + interest)
            };
            var account = new Account("AC001", new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 100, 100),
            });
            var rules = new List<InterestRule>()
            {
                new InterestRule(new DateOnly(2023, 10, 03), 10m),
                new InterestRule(new DateOnly(2023, 10, 15), 20m),
            };

            var statement = new Statement(account, rules, new DateOnly(2023, 10, 01));

            var statementTransactions = statement.Transactions;
            Assert.Equal("AC001", statement.Id);
            Assert.Equal(2, statementTransactions.Count());
            Assert.Equal(expected, statementTransactions.ToList());
        }

        [Fact]
        public void Statement_for_complexe_account_and_rules()
        {
            var expected = new List<Transaction>()
            {
                new Transaction("20230601-01", new DateOnly(2023, 06, 01), "D",   150, 250),
                new Transaction("20230626-01", new DateOnly(2023, 06, 26), "W", 20, 230),
                new Transaction("20230626-02", new DateOnly(2023, 06, 26), "W", 100, 130),
                new Transaction("", new DateOnly(2023, 06, 30), "I", 0.39m, 130.39m)
            };
            var account = new Account("AC001", new List<Transaction>()
            {
                new Transaction("20230505-01", new DateOnly(2023, 05, 05), "D", 100, 100),
                new Transaction("20230601-01", new DateOnly(2023, 06, 01), "D", 150, 250),
                new Transaction("20230626-01", new DateOnly(2023, 06, 26), "W", 20, 230),
                new Transaction("20230626-02", new DateOnly(2023, 06, 26), "W", 100, 130),
            });
            var rules = new List<InterestRule>()
            {
                new InterestRule(new DateOnly(2023, 01, 01), 1.95m),
                new InterestRule(new DateOnly(2023, 05, 20), 1.90m),
                new InterestRule(new DateOnly(2023, 06, 15), 2.20m),
            };
            var statement = new Statement(account, rules, new DateOnly(2023, 06, 01));

            var statementTransactions = statement.Transactions;
            Assert.Equal("AC001", statement.Id);
            Assert.Equal(4, statementTransactions.Count());
            Assert.Equal(expected, statementTransactions.ToList());
        }
    }
}
