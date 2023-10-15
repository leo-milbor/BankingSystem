using BankingSystem.Account.UseCases;
using BankingSystem.Fromatter;

namespace BankingSystemTests.FormatterTests
{
    public class AccountFormatterTests
    {
        [Fact]
        public void Format_account()
        {
            var expected = @"Account: AC001
| Date     | Txn Id      | Type | Amount |
| 20230505 | 20230505-01 | D    | 100.00 |
| 20230601 | 20230601-01 | D    | 150.00 |
| 20230626 | 20230626-01 | W    |  20.00 |
| 20230626 | 20230626-02 | W    | 100.00 |";
            var account = new AccountDTO("AC001",
                new List<TransactionDTO>()
                {
                    new TransactionDTO(1, new DateOnly(2023, 05, 05), "D", 100m),
                    new TransactionDTO(1, new DateOnly(2023, 06, 1), "D", 150m),
                    new TransactionDTO(1, new DateOnly(2023, 06, 26), "W", 20m),
                    new TransactionDTO(2, new DateOnly(2023, 06, 26), "W", 100m),
                });
            var formatter = new AccountFormatter();

            var result = formatter.Format(account);

            Assert.Equal(expected, result);
        }
    }
}
