using System.Globalization;

namespace BankingSystem.Statement.UseCases
{
    interface IAccountRepository
    {
        Account? Get(string Id);
    }
    interface IInterestRuleRepository
    {
        IEnumerable<InterestRule> GetAll();
    }
    internal record TransactionDTO(DateOnly Date, int RunningNumber, string Type, decimal Amount, decimal Balance);
    internal record StatementDTO(string Id, IEnumerable<TransactionDTO> Transactions);
    internal class PrintStatementUseCase : IUseCase<StatementDTO>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IInterestRuleRepository _interestRuleRepository;

        public PrintStatementUseCase(IAccountRepository accountRepository, IInterestRuleRepository interestRuleRepository)
        {
            _accountRepository = accountRepository;
            _interestRuleRepository = interestRuleRepository;
        }

        public StatementDTO Apply(string input)
        {
            var inputs = input.Split(' ');
            if (inputs.Length != 2)
                throw new UseCaseException("Wrong number of argument to get a statement.");


            var isCorrectDate = DateTime.TryParseExact(inputs[1], "yyyyMM", SingaporeanFormatProvider.Instance, DateTimeStyles.AssumeLocal, out var date);
            if (!isCorrectDate)
                throw new UseCaseException("Invalid date, should be in YYYYMM format.");
            var dateOnly = new DateOnly(date.Year, date.Month, 01);

            var account = _accountRepository.Get(inputs[0]);
            if (account is null)
                throw new UseCaseException("Unknown account.");
            var rules = _interestRuleRepository.GetAll();

            var statement = new Statement(account, rules, dateOnly);

            return new StatementDTO(
                statement.Id,
                statement.Transactions
                .Select(s => new TransactionDTO(s.Date, s.RunningNumber, s.Type, s.Amount, s.Balance)));
        }
    }
}
