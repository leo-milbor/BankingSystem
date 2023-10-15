using BankingSystem.Statement;
using BankingSystem.Statement.UseCases;

namespace BankingSystemTests.StatementTests.UseCasesTests
{
    internal record InMemoryInterestRuleRepository(IEnumerable<InterestRule> Rules) : IInterestRuleRepository
    {
        public IEnumerable<InterestRule> GetAll()
        {
            return Rules;
        }
    }
}
