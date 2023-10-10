namespace BankingSystem.InterestRule.UseCases
{
    internal interface IInterestRuleRepository
    {
        void Add(InterestRule rule);
        ISet<InterestRule> GetAll();
    }
    internal class DefineInterestRuleUseCase
    {
        private readonly IInterestRuleRepository _repository;

        public DefineInterestRuleUseCase(IInterestRuleRepository repository)
        {
            _repository = repository;
        }

        public string Apply(string input)
        {
            return "";
        }
    }
}
