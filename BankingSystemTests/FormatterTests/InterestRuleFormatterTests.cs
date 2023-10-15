using BankingSystem.Fromatter;
using BankingSystem.InterestRule.UseCases;

namespace BankingSystemTests.FormatterTests
{
    public class InterestRuleFormatterTests
    {
        [Fact]
        public void Format_rules()
        {
            var expected = @"Interest rules:
| Date     | RuleId | Rate (%) |
| 20230101 | RULE01 |     1.95 |
| 20230520 | RULE02 |     1.90 |
| 20230615 | RULE03 |     2.20 |";
            var rules = new InterestRuleDTO[3]
            {
                new InterestRuleDTO("RULE01", new DateOnly(2023, 01, 01), 1.95m),
                new InterestRuleDTO("RULE02", new DateOnly(2023, 05, 20), 1.90m),
                new InterestRuleDTO("RULE03", new DateOnly(2023, 06, 15), 2.20m)
            };
            var formatter = new InterestRulesFormatter();

            var result = formatter.Format(rules);

            Assert.Equal(expected, result);
        }
    }
}
