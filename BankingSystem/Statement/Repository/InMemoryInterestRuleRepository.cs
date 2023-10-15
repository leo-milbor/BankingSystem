using System.Data;

using BankingSystem.Statement.UseCases;

using RuleUS = BankingSystem.InterestRule;

namespace BankingSystem.Statement.Repository
{
    internal class InMemoryInterestRuleRepository : IInterestRuleRepository
    {
        private readonly RuleUS.UseCases.IInterestRuleRepository _repository;

        public InMemoryInterestRuleRepository(RuleUS.UseCases.IInterestRuleRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<InterestRule> GetAll()
        {
            return _repository.GetAll().Select(r => new InterestRule(r.Date, r.Rate));
        }
    }
}
