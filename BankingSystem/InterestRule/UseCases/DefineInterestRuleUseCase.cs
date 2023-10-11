
using static BankingSystem.InterestRule.Date;
using static BankingSystem.InterestRule.Rate;

namespace BankingSystem.InterestRule.UseCases
{
    internal interface IInterestRuleRepository
    {
        void Upsert(InterestRule rule);
        ISet<InterestRule> GetAll();
    }
    internal record InterestRuleDTO(string Id, DateOnly Date, decimal Rate);

    internal class DefineInterestRuleUseCase : IUseCase<IEnumerable<InterestRuleDTO>>
    {
        private readonly IInterestRuleRepository _repository;

        public DefineInterestRuleUseCase(IInterestRuleRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<InterestRuleDTO> Apply(string input)
        {
            var inputs = input.Split(' ');
            if (inputs.Length != 3)
                throw new UseCaseException("Wrong number of argument to define an interest rule.");

            var rule = TryParse(inputs);

            _repository.Upsert(rule);

            var allRules = _repository.GetAll()
                .Select(r => new InterestRuleDTO(r.Id, r.Date, r.Rate));
            return allRules;
        }

        private static InterestRule TryParse(string[] inputs)
        {
            InterestRule interestRule;
            try
            {
                Date date = new Date(inputs[0]);
                string id = inputs[1];
                Rate rate = new Rate(inputs[2]);
                interestRule = new InterestRule(id, date, rate);
            }
            catch (Exception e)
            {
                var message = e switch
                {
                    NotAValidDateFormatException => "Invalid date, should be in YYYYMMdd format.",
                    NotAValidDecimalException => "Invalid rate, should be a correct decimal number.",
                    OutOfRangeException => "Invalid rate, should be greater than 0 and less than 100.",
                    _ => "An unknown error occured.",
                };
                throw new UseCaseException(message);
            }
            return interestRule;
        }
    }
}
