using BankingSystem.InterestRule.UseCases;
using BankingSystem;
using BankingSystem.Statement;
using BankingSystem.Statement.UseCases;

namespace BankingSystemTests.StatementTests.UseCasesTests
{
    public class PrintStatementUseCaseTests
    {
        [Fact]
        public void User_can_request_to_print_a_statement_for_an_account_for_a_month()
        {
            var expected = new StatementDTO(
                "AC001",
                new List<TransactionDTO>()
                {
                    new TransactionDTO(new DateOnly(2023, 06,01), 1, "D", 150,250),
                    new TransactionDTO(new DateOnly(2023, 06,26), 1, "W", 20,230),
                    new TransactionDTO(new DateOnly(2023, 06,26), 2, "W", 100,130),
                    new TransactionDTO(new DateOnly(2023, 06,30), 1, "I", 0.39m,130.39m),
                });
            var accountRepository = new InMemoryAccountRepository(
                new Account(
                    "AC001",
                    new List<Transaction>()
                    {
                        new Transaction("20230505-01", new DateOnly(2023, 05, 05), 1, "D", 100, 100),
                        new Transaction("20230601-01", new DateOnly(2023, 06, 01), 1, "D", 150, 250),
                        new Transaction("20230626-01", new DateOnly(2023, 06, 26), 1, "W", 20, 230),
                        new Transaction("20230626-02", new DateOnly(2023, 06, 26), 2, "W", 100, 130),
                    }));
            var interestRuleRepository = new InMemoryInterestRuleRepository(
                new List<InterestRule>()
                {
                    new InterestRule(new DateOnly(2023, 01, 01), 1.95m),
                    new InterestRule(new DateOnly(2023, 05, 20), 1.90m),
                    new InterestRule(new DateOnly(2023, 06, 15), 2.20m),
                });
            var useCase = new PrintStatementUseCase(accountRepository, interestRuleRepository);

            var statement = useCase.Apply("AC001 202306");

            Assert.Equal(expected.Id, statement.Id);
            Assert.Equal(expected.Transactions.ToList(), statement.Transactions.ToList());
        }

        [Theory]
        [InlineData("", "Wrong number of argument to get a statement.")]
        [InlineData("AC001 xyj", "Invalid date, should be in YYYYMM format.")]
        [InlineData("NotAnAccount 202306", "Unknown account.")]
        public void User_has_feedback_on_invalid_input(string input, string expectedMessage)
        {
            var accountRepository = new InMemoryAccountRepository(null);
            var interestRuleRepository = new InMemoryInterestRuleRepository(
                new List<InterestRule>()
                {
                    new InterestRule(new DateOnly(2023, 01, 01), 1.95m),
                    new InterestRule(new DateOnly(2023, 05, 20), 1.90m),
                    new InterestRule(new DateOnly(2023, 06, 15), 2.20m),
                });
            var useCase = new PrintStatementUseCase(accountRepository, interestRuleRepository);
            try
            {
                useCase.Apply(input);
                throw new Exception("Should have failed.");
            }
            catch (UseCaseException e)
            {
                Assert.Equal(expectedMessage, e.Message);
            }
        }
    }
}
