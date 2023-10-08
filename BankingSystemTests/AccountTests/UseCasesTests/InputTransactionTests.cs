using System.Text;

using BankingSystem.Account.UseCases;

namespace BankingSystemTests.AccountTests.UseCasesTests
{
    public class InputTransactionTests
    {
        [Fact]
        public void User_can_create_an_account_on_first_transaction()
        {
            var expectedOutput = new StringBuilder()
                .Append("Account: AC001").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount |").AppendLine()
                .Append("| 20230626 | 20230626-01 | D    | 100.00 |")
                .ToString();
            var accounRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accounRepository);

            var output = useCase.Apply("20230626 AC001 D 100.00");

            Assert.Equal(expectedOutput, output);
            var dbAccounts = accounRepository.GetAll();
            Assert.Equal(1, dbAccounts.Count);
            Assert.Equal(expectedOutput, dbAccounts.First().ToString());
        }

        [Fact]
        public void User_can_input_transactions()
        {
            var expectedOutput = new StringBuilder()
                .Append("Account: AC001").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount |").AppendLine()
                .Append("| 20230505 | 20230505-01 | D    | 100.00 |").AppendLine()
                .Append("| 20230601 | 20230601-01 | D    | 150.00 |").AppendLine()
                .Append("| 20230626 | 20230626-01 | W    |  20.00 |").AppendLine()
                .Append("| 20230626 | 20230626-02 | W    | 100.00 |")
                .ToString();
            var accounRepository = new InMemoryAccountRepository();
            var useCase = new InputTransactionUseCase(accounRepository);

            useCase.Apply("20230505 AC002 D 100.00");

            useCase.Apply("20230505 AC001 D 100.00");
            useCase.Apply("20230601 AC001 D 150.00");
            useCase.Apply("20230626 AC001 W 20.00");
            var output = useCase.Apply("20230626 AC001 W 100.00");

            Assert.Equal(expectedOutput, output);
            var dbAccounts = accounRepository.GetAll();
            Assert.Equal(2, dbAccounts.Count);
            var ac001 = accounRepository.Get("AC001");
            Assert.NotNull(ac001);
            Assert.Equal(expectedOutput, ac001.ToString());
        }
    }
}
