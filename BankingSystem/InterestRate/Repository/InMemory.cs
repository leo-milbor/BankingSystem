using BankingSystem.InterestRule.UseCases;

namespace BankingSystem.InterestRule.Repository
{
    internal class InMemory : IInterestRuleRepository
    {
        public void Add(InterestRule rule)
        {
            throw new NotImplementedException();
        }

        public ISet<InterestRule> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
