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
            var dbActual = ToDTO(dbAccounts.First(a => a.Name == "AC001"));
            Assert.Equal(expectedOutput.Id, dbActual.Id);
            Assert.Equal(expectedOutput.Transactions, dbActual.Transactions.ToList());
        }

        private static AccountDTO ToDTO(Account account)
        {
            var transactions = account.Transactions.Select(ToDTO);
            return new AccountDTO(account.Name, transactions);
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
