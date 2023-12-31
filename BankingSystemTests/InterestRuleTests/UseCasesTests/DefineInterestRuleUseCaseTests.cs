﻿using BankingSystem;
using BankingSystem.InterestRule;
using BankingSystem.InterestRule.UseCases;

namespace BankingSystemTests.InterestRuleTests.UseCasesTests
{
    public class DefineInterestRuleUseCaseTests
    {
        [Fact]
        public void User_can_define_new_interest_rule()
        {
            IEnumerable<InterestRuleDTO> expectedOutput = new InterestRuleDTO[1]
            {
                new InterestRuleDTO("RULE01", new DateOnly(2023, 01, 01), 1.95m)
            };
            var ruleRepository = new InMemoryInterestRuleRepository();
            var useCase = new DefineInterestRuleUseCase(ruleRepository);

            var output = useCase.Apply("20230101 RULE01 1.95");

            Assert.Equal(expectedOutput, output);
            var dbRules = ruleRepository.GetAll();
            Assert.Equal(1, dbRules.Count);
            var dbInterestRule = dbRules.First();
            Assert.Equal(expectedOutput.First(), ToDTO(dbInterestRule));
        }

        [Fact]
        public void A_new_interest_rule_at_a_same_date_of_a_previous_one_replaces_it()
        {
            var expectedOutput = new InterestRuleDTO[3]
            {
                new InterestRuleDTO("RULE01", new DateOnly(2023, 01, 01), 1.95m),
                new InterestRuleDTO("RULE04", new DateOnly(2023, 05, 20), 2.05m),
                new InterestRuleDTO("RULE03", new DateOnly(2023, 06, 15), 2.20m)
            };
            var ruleRepository = new InMemoryInterestRuleRepository();
            var useCase = new DefineInterestRuleUseCase(ruleRepository);
            useCase.Apply("20230101 RULE01 1.95");
            useCase.Apply("20230520 RULE02 1.90");
            useCase.Apply("20230615 RULE03 2.20");
            var dbRules = ruleRepository.GetAll();
            Assert.Equal(3, dbRules.Count);

            var output = useCase.Apply("20230520 RULE04 2.05");

            Assert.Equal(expectedOutput, output.ToArray());
            dbRules = ruleRepository.GetAll();
            Assert.Equal(3, dbRules.Count);
        }

        [Theory]
        [InlineData("", "Wrong number of argument to define an interest rule.")]
        [InlineData("2023/01/01 RULE01 1.95", "Invalid date, should be in YYYYMMdd format.")]
        [InlineData("20230101 RULE01 two", "Invalid rate, should be a correct decimal number.")]
        [InlineData("20230101 RULE01 195", "Invalid rate, should be greater than 0 and less than 100.")]
        public void User_has_feedback_on_invalid_input(string input, string expectedMessage)
        {
            var ruleRepository = new InMemoryInterestRuleRepository();
            var useCase = new DefineInterestRuleUseCase(ruleRepository);
            try
            {
                useCase.Apply(input);
                throw new Exception("Should have failed.");
            }
            catch (UseCaseException e)
            {
                Assert.Equal(expectedMessage, e.Message);
            }
        }

        private static InterestRuleDTO ToDTO(InterestRule interestRule) =>
            new(interestRule.Id, interestRule.Date, interestRule.Rate);
    }
}
