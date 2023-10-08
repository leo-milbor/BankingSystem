using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class TransactionTests
    {
        [Theory]
        [InlineData("123", "123.00")]
        [InlineData("123.67", "123.67")]
        [InlineData("3.6", "  3.60")]
        public void Transaction_can_be_printed(string rawAmount, string expectedAmount)
        {
            var date = new Date("20231007");
            var type = new TransactionType("d");
            var amount = new Amount(rawAmount);
            var t = new Transaction(1, date, type, amount);

            Assert.Equal($"| 20231007 | 20231007-01 | D    | {expectedAmount} |", t.ToString());
        }
    }
}
