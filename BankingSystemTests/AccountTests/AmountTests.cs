using System.Globalization;

using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class AmountTests
    {
        [Theory]
        [InlineData(100, "100.00")]
        [InlineData(50, " 50.00")]
        [InlineData(235.67, "235.67")]
        public void Correct_amount(decimal value, string expected)
        {
            var amount = new Amount(formatDecimal(value, "###.##"));

            Assert.Equal(expected, amount.ToString());
        }

        [Theory]
        [InlineData(-100, "000")]
        [InlineData(0, "0")]
        [InlineData(0, "-0")]
        [InlineData(0, "-0.00")]
        [InlineData(-1, "0.00")]
        public void Amount_must_be_greater_than_zero(decimal value, string format)
        {
            Assert.Throws<Amount.NegativeAmountException>(() => new Amount(formatDecimal(value, format)));
        }

        [Theory]
        [InlineData(10.123, "00.000")]
        [InlineData(10.000, "00.000")]
        [InlineData(0.001, "0.000")]
        public void Amount_cannot_have_too_many_decimals(decimal value, string format)
        {
            Assert.Throws<Amount.TooManyDecimalsException>(() => new Amount(formatDecimal(value, format)));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("not a number")]
        [InlineData("")]
        [InlineData("\n")]
        public void Amount_must_be_a_number(string amount)
        {
            Assert.Throws<Amount.NotAValidDecimalException>(() => new Amount(amount));
        }

        private static string formatDecimal(decimal value, string format) => value.ToString(format, new CultureInfo("en-US"));
    }
}
