using System.Diagnostics.CodeAnalysis;

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
        public InMemoryInterestRuleRepository()
            : this(new HashSet<InterestRule>(new InterestRulePerDateComparer())) { }

        public void Upsert(InterestRule rule)
        {
            if (_interestRules.Contains(rule))
                _interestRules.Remove(rule);
            _interestRules.Add(rule);
        }

        public ISet<InterestRule> GetAll()
        {
            return new HashSet<InterestRule>(_interestRules);
        }
    }
    internal class InterestRulePerDateComparer : IEqualityComparer<InterestRule>
    {
        public bool Equals(InterestRule? x, InterestRule? y)
        {
            if (x == null && y == null) return false;
            if (x == null || y == null) return false;
            return x.Date.Equals(y.Date);
        }


        public int GetHashCode([DisallowNull] InterestRule obj)
             => obj.Date.GetHashCode();
    }
}
