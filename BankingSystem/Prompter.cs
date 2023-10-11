using AccountRepo = BankingSystem.Account.Repository;
using BankingSystem.Account.UseCases;
using BankingSystem.InterestRule.UseCases;
using InterestRuleRepo = BankingSystem.InterestRule.Repository;
using BankingSystem.Fromatter;

namespace BankingSystem
{
    internal interface IWriter
    {
        void Write(string value);
    }
    internal interface IReader
    {
        string Read();
    }
    internal interface IReadWriter : IReader, IWriter { }

    internal class Prompter
    {
        private readonly IReadWriter _readWriter;
        private readonly InputTransactionUseCase _inputTransactionUseCase;
        private readonly DefineInterestRuleUseCase _defineInterestRuleUseCase;

        public Prompter(IReadWriter readWriter)
        {
            _readWriter = readWriter;
            _inputTransactionUseCase = new InputTransactionUseCase(new AccountRepo.InMemory());
            _defineInterestRuleUseCase = new DefineInterestRuleUseCase(new InterestRuleRepo.InMemory());
        }


        public void Launch()
        {
            _readWriter.Write(_launching);
            Loop();
        }

        private void Loop()
        {
            while (true)
            {
                var value = _readWriter.Read().ToUpper();
                var prompt = NextPrompt(value);
                var result = "";
                switch (prompt)
                {
                    case _inputTransaction:
                        result = PlayUseCase(prompt, _inputTransactionUseCase, AccountFormatter.Instance);
                        break;
                    case _interestRules:
                        result = PlayUseCase(prompt, _defineInterestRuleUseCase, InterestRulesFormatter.Instance);
                        break;
                    case _printStatement:
                        break;
                    case _goodbye:
                        _readWriter.Write(prompt);
                        return;
                    default:
                        result = _invalidChoice;
                        break;
                }
                _readWriter.Write(result + $"\r\n{_followup}");
            }
        }

        private string PlayUseCase<TOut>(string prompt, IUseCase<TOut> useCase, IFormatter<TOut> formatter)
        {
            _readWriter.Write(prompt);
            var input = _readWriter.Read();
            try
            {
                var result = useCase.Apply(input);
                return formatter.Format(result);
            }
            catch (UseCaseException e)
            {
                return e.Message;
            }
            catch
            {
                return "An unknown error occured.";
            }
        }

        private string NextPrompt(string value) => value switch
        {
            "T" => _inputTransaction,
            "I" => _interestRules,
            "P" => _printStatement,
            "Q" => _goodbye,
            _ => _launching,
        };

        private const string _launching = @"Welcome to AwesomeGIC Bank! What would you like to do?" + _inputChoices;

        private const string _invalidChoice = @"Invalid choice.";

        private const string _followup = @"Is there anything else you'd like to do?" + _inputChoices;

        private const string _inputChoices = @"
[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit
>";

        private const string _inputTransaction = @"Please enter transaction details in <Date> <Account> <Type> <Amount> format 
(or enter blank to go back to main menu):
>";

        private const string _interestRules = @"Please enter interest rules details in <Date> <RuleId> <Rate in %> format 
(or enter blank to go back to main menu):
>";

        private const string _printStatement = @"Please enter account and month to generate the statement <Account> <Year><Month>
(or enter blank to go back to main menu):
>";

        private const string _goodbye = @"Thank you for banking with AwesomeGIC Bank.
Have a nice day!";
    }
}
