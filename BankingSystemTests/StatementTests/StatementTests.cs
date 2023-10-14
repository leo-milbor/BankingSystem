using BankingSystem.Statement;

namespace BankingSystemTests.StatementTests
{
    public class StatementTests
    {
        [Fact]
        public void Statement_for_1_account_1_interest_rule()
        {
            const decimal interest = 100 * 0.1m * 21 / 365;
            var expected = new List<Transaction>()
            {
                new Transaction("20231010-01", new DateOnly(2023, 10, 10), "D", 100, 100),
                new Transaction("", new DateOnly(2023, 10, 31), "I", 100 + interest, interest)
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
    }
}
