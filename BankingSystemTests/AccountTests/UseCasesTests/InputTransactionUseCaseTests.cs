using BankingSystem;
using BankingSystem.Account;
using BankingSystem.Account.UseCases;

namespace BankingSystemTests.AccountTests.UseCasesTests
{
    public class InputTransactionUseCaseTests
    {
        [Fact]
        public void User_can_create_an_account_on_first_transaction()
        {
            var expectedOutput = new AccountDTO("AC001",
                new List<TransactionDTO>()
                { new TransactionDTO(1, new DateOnly(2023, 06, 26), "D", 100.00m)});
            var accountRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accountRepository);

            var output = useCase.Apply("20230626 AC001 D 100.00");

            Assert.Equal(expectedOutput.Id, output.Id);
            Assert.Equal(expectedOutput.Transactions, output.Transactions.ToList());
            var dbAccounts = accountRepository.GetAll();
            Assert.Equal(1, dbAccounts.Count);
            var dbActual = ToDTO(dbAccounts.First());
            Assert.Equal(expectedOutput.Id, dbActual.Id);
            Assert.Equal(expectedOutput.Transactions, dbActual.Transactions.ToList());
        }

        [Fact]
        public void User_can_input_transactions()
        {
            var expectedOutput = new AccountDTO("AC001",
                new List<TransactionDTO>()
                {
                    new TransactionDTO(1, new DateOnly(2023, 05, 05), "D", 100m),
                    new TransactionDTO(1, new DateOnly(2023, 06, 1), "D", 150m),
                    new TransactionDTO(1, new DateOnly(2023, 06, 26), "W", 20m),
                    new TransactionDTO(2, new DateOnly(2023, 06, 26), "W", 100m),
                });
            var accountRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accountRepository);

            useCase.Apply("20230505 AC002 D 100.00");
            useCase.Apply("20230505 AC001 D 100.00");
            useCase.Apply("20230601 AC001 D 150.00");
            useCase.Apply("20230626 AC001 W 20.00");
            var output = useCase.Apply("20230626 AC001 W 100.00");

            Assert.Equal(expectedOutput.Id, output.Id);
            Assert.Equal(expectedOutput.Transactions, output.Transactions.ToList());
            var dbAccounts = accountRepository.GetAll();
            Assert.Equal(2, dbAccounts.Count);
            var dbActual = ToDTO(dbAccounts.First(a => a.Id == "AC001"));
            Assert.Equal(expectedOutput.Id, dbActual.Id);
            Assert.Equal(expectedOutput.Transactions, dbActual.Transactions.ToList());
        }

        [Fact]
        public void User_cannot_input_transaction_to_negative_balance()
        {
            var expectedTransaction = new TransactionDTO(1, new DateOnly(2023, 05,05), "D", 100);
            var accountRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accountRepository);

            useCase.Apply("20230505 AC002 D 100.00");

            Assert.Throws<UseCaseException>(() => useCase.Apply("20230626 AC002 W 150.00"));

            var dbAccounts = accountRepository.GetAll();
            var dbActual = ToDTO(Assert.Single(dbAccounts));
            Assert.Equal("AC002", dbActual.Id);
            Assert.Equal(expectedTransaction, Assert.Single(dbActual.Transactions));
        }

        [Theory]
        [InlineData("xyz", "Wrong number of argument to create an account.")]
        [InlineData("2023/05/05 AC002 D 100.00", "Invalid date, should be in YYYYMMdd format.")]
        [InlineData("20230505 AC002 Z 100.00", "Invalid type, D for deposit, W for withdrawal.")]
        [InlineData("20230505 AC002 D hundred", "Invalid amount, should be a correct decimal number.")]
        [InlineData("20230505 AC002 D 100.153", "Invalid amount, decimals are allowed up to 2 decimal places.")]
        [InlineData("20230505 AC002 D -100.00", "Invalid amount, must be greater than zero.")]
        [InlineData("20230505 AC002 W 100.00", "Invalid transaction, balance should not be less than 0.")]
        public void User_has_feedback_on_invalid_input(string input, string expectedMessage)
        {
            var accountRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accountRepository);
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

        private static AccountDTO ToDTO(Account account)
        {
            var transactions = account.Transactions.Select(ToDTO);
            return new AccountDTO(account.Id, transactions);
        }

        private static TransactionDTO ToDTO(Transaction transaction)
        {
            return new TransactionDTO(
                transaction.RunningNumber,
                transaction.Date.Value,
                transaction.Type,
                transaction.Amount);
        }
    }
}
