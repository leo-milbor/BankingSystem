using System.Diagnostics.CodeAnalysis;

using BankingSystem.InterestRule;
using BankingSystem.InterestRule.UseCases;

namespace BankingSystemTests.InterestRuleTests.UseCasesTests
{
    internal class InMemoryInterestRuleRepository : IInterestRuleRepository
    {
        private readonly ISet<InterestRule> _interestRules;
        public void Add(InterestRule rule)
        {
            throw new NotImplementedException();
        }

        public ISet<InterestRule> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    internal class InterestRuleComparer : IEqualityComparer<InterestRule>
    {
        public bool Equals(InterestRule? x, InterestRule? y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] InterestRule obj)
        {
            throw new NotImplementedException();
        }
    }
}
