using System.Text;

using BankingSystem.InterestRule.UseCases;

namespace BankingSystemTests.InterestRuleTests.UseCasesTests
{
    public class DefineInterestRuleUseCaseTests
    {
        [Fact]
        public void User_can_define_new_interest_rule()
        {
            var expectedOutput = new StringBuilder()
                .Append("Interest rules:").AppendLine()
                .Append("| Date | RuleId | Rate(%) |").AppendLine()
                .Append("| 20230101 | RULE01 | 1.95 |")
                .ToString();
            var ruleRepository = new InMemoryInterestRuleRepository();
            var useCase = new DefineInterestRuleUseCase(ruleRepository);

            var output = useCase.Apply("20230101 RULE01 1.95");

            Assert.Equal(expectedOutput, output);
            var dbRules = ruleRepository.GetAll();
            Assert.Equal(1, dbRules.Count);
            Assert.Equal(expectedOutput, dbRules.First().ToString());
        }

        [Fact]
        public void A_new_interest_rule_at_a_same_date_of_a_previous_one_replaces_it()
        {
            var expectedOutput = new StringBuilder()
                .Append("Interest rules:").AppendLine()
                .Append("| Date | RuleId | Rate(%) |").AppendLine()
                .Append("| 20230101 | RULE01 | 1.95 |").AppendLine()
                .Append("| 20230520 | RULE04 | 2.05 |").AppendLine()
                .Append("| 20230615 | RULE03 | 2.20 |")
                .ToString();
            var ruleRepository = new InMemoryInterestRuleRepository();
            var useCase = new DefineInterestRuleUseCase(ruleRepository);
            useCase.Apply("20230101 RULE01 1.95");
            useCase.Apply("20230520 RULE02 1.90");
            useCase.Apply("20230615 RULE03 2.20");
            var dbRules = ruleRepository.GetAll();
            Assert.Equal(3, dbRules.Count);

            var output = useCase.Apply("20230520 RULE04 2.05");

            Assert.Equal(expectedOutput, output);
            dbRules = ruleRepository.GetAll();
            Assert.Equal(3, dbRules.Count);
        }
    }
}
