using System.Globalization;

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

        public Prompter(IReadWriter readWriter)
        {
            _readWriter = readWriter;
        }


        public void Launch()
        {
            _readWriter.Write(_launchingPrompt);
            var value = _readWriter.Read().ToUpper();
            _readWriter.Write(NextPrompt(value));
        }

        private string NextPrompt(string value) => value switch
        {
            "T" => _inputTransactionPrompt,
            "I" => _interestRulesPrompt,
            "P" => _printStatementPrompt,
            _ => _launchingPrompt,
        };

        private const string _launchingPrompt = @"Welcome to AwesomeGIC Bank! What would you like to do?
" + _inputChoices;

        private const string _followupPrompt = @"Is there anything else you'd like to do?" + _inputChoices;

        private const string _inputChoices = @"[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit";

        private const string _inputTransactionPrompt = @"Please enter transaction details in <Date> <Account> <Type> <Amount> format 
(or enter blank to go back to main menu):";

        private const string _interestRulesPrompt = @"Please enter interest rules details in <Date> <RuleId> <Rate in %> format 
(or enter blank to go back to main menu):";

        private const string _printStatementPrompt = @"Please enter account and month to generate the statement <Account> <Year><Month>
(or enter blank to go back to main menu):";
    }
}
