using BankingSystem.InterestRule;

namespace BankingSystemTests.InterestRuleTests
{
    public class RateTests
    {
        [Theory]
        [InlineData("0")]
        [InlineData("100")]
        [InlineData("-10.3")]
        [InlineData("150")]
        public void Cannot_create_rate_at_0_or_less_or_100_or_more(string rawRate)
        {
            Assert.Throws<Rate.OutOfRangeException>(() => new Rate(rawRate));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("not a number")]
        [InlineData("")]
        [InlineData("\n")]
        public void Rate_must_be_a_number(string rawRate)
        {
            Assert.Throws<Rate.NotAValidDecimalException>(() => new Rate(rawRate));
        }

        [Theory]
        [InlineData("0.39", 0.39)]
        [InlineData("15", 15.00)]
        [InlineData("99.9", 99.90)]
        public void Can_create_rate_between_0_and_100(string rawRate, decimal expected)
        {
            var r = new Rate(rawRate);

            Assert.Equal(expected, r.Value);
        }
    }
}
