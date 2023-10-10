namespace BankingSystem.InterestRule.UseCases
{
    internal interface IInterestRuleRepository
    {
        void Upsert(InterestRule rule);
        IEnumerable<InterestRule> GetAll();
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
            var inputs = input.Split(' ');
            if (inputs.Length != 3)
                return "Not enough argument to define an interest rule.";

            Date date = new Date(inputs[0]);
            string id = inputs[1];
            Rate rate = new Rate(inputs[2]);
            var rule = new InterestRule(id, date, rate);

            _repository.Upsert(rule);

            return "";
        }
    }
}
