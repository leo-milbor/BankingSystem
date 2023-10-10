using BankingSystem.InterestRule;

namespace BankingSystemTests.InterestRuleTests
{
    public class InterestRuleTests
    {
        [Fact]
        public void Interest_rule_can_be_printed()
        {
            var date = new Date("20230615");
            var rate = new Rate("2.20");
            var rule = new InterestRule("RULE03", date, rate);

            Assert.Equal("| 20230615 | RULE03 |     2.20 |", rule.ToString());
        }
    }
}
