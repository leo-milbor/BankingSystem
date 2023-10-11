using BankingSystem;
using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class AmountTests
    {
        [Theory]
        [InlineData("100", 100.00)]
        [InlineData("50", 50.00)]
        [InlineData("235.67", 235.67)]
        public void Correct_amount(string value, decimal expected)
        {
            var amount = new Amount(value);

            Assert.Equal(expected, amount.Value);
        }

        [Theory]
        [InlineData("-100")]
        [InlineData("0")]
        [InlineData("-0")]
        [InlineData("-0.00")]
        [InlineData("-1")]
        public void Amount_must_be_greater_than_zero(string value)
        {
            Assert.Throws<Amount.NegativeAmountException>(() => new Amount(value));
        }

        [Theory]
        [InlineData("10.123")]
        [InlineData("10.000")]
        [InlineData("0.001")]
        public void Amount_cannot_have_too_many_decimals(string value)
        {
            Assert.Throws<Amount.TooManyDecimalsException>(() => new Amount(value));
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

        private static string formatDecimal(decimal value, string format) => value.ToString(format, SingaporeanFormatProvider.Instance);
    }
}
