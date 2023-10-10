using BankingSystem.InterestRule;
using BankingSystem.InterestRule.UseCases;

namespace BankingSystemTests.InterestRuleTests.UseCasesTests
{
    internal class InMemoryInterestRuleRepository : IInterestRuleRepository
    {
        private readonly ISet<InterestRule> _interestRules;

        private InMemoryInterestRuleRepository(ISet<InterestRule> interestRules)
        {
            _interestRules = interestRules;
        }
        public InMemoryInterestRuleRepository() : this(new HashSet<InterestRule>()) { }

        public void Upsert(InterestRule rule)
        {
            _interestRules.Add(rule);
        }

        public ISet<InterestRule> GetAll()
        {
            return new HashSet<InterestRule>(_interestRules);
        }
    }
}
