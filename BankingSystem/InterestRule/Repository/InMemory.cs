using BankingSystem.InterestRule.UseCases;

namespace BankingSystem.InterestRule.Repository
{
    internal class InMemory : IInterestRuleRepository
    {
        private readonly ISet<InterestRule> _interestRules;

        private InMemory(ISet<InterestRule> interestRules)
        {
            _interestRules = interestRules;
        }
        public InMemory() : this(new HashSet<InterestRule>()) { }

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
