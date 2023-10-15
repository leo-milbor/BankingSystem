using BankingSystem.Fromatter;
using BankingSystem.Statement.UseCases;

namespace BankingSystemTests.FormatterTests
{
    public class PrintStatementFormatterTests
    {
        [Fact]
        public void Format_Statement()
        {
            var expected = @"Account: AC001
| Date     | Txn Id      | Type | Amount | Balance |
| 20230601 | 20230601-01 | D    | 150.00 |  250.00 |
| 20230626 | 20230626-01 | W    |  20.00 |  230.00 |
| 20230626 | 20230626-02 | W    | 100.00 |  130.00 |
| 20230630 |             | I    |   0.39 |  130.39 |";
            var statement = new StatementDTO(
                "AC001",
                new List<TransactionDTO>()
                {
                    new TransactionDTO(new DateOnly(2023, 06,01), 1, "D", 150,250),
                    new TransactionDTO(new DateOnly(2023, 06,26), 1, "W", 20,230),
                    new TransactionDTO(new DateOnly(2023, 06,26), 2, "W", 100,130),
                    new TransactionDTO(new DateOnly(2023, 06,30), 1, "I", 0.39m,130.39m),
                });

            var formatter = new PrintStatementFormatter();

            var result = formatter.Format(statement);

            Assert.Equal(expected, result);
        }
    }
}
