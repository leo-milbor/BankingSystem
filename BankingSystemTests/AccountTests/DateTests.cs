using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class DateTests
    {
        [Fact]
        public void Date_20230626_is_valid()
        {
            var date = new Date("20230626");

            Assert.Equal("20230626", date.ToString());
        }

        [Theory]
        [InlineData("2023/10/07")]
        [InlineData("2023-10-07")]
        [InlineData("07102023")]
        [InlineData("10072023")]
        public void Date_must_use_YYYYMMdd_format(string date)
        {
            Assert.Throws<Date.NotAValidDateFormatException>(() => new Date(date));
        }
    }
}
