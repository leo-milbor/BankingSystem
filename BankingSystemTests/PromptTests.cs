using BankingSystem;

namespace BankingSystemTests
{
    public class PromptTests
    {
        [Fact]
        public void New_prompt_prints_welcome_message()
        {
            const string Expected = @"Welcome to AwesomeGIC Bank! What would you like to do?
[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit
>";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock();
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(Expected, writer.GetOutputNumber(1));
        }

        [Theory]
        [InlineData("Q")]
        [InlineData("q")]
        public void User_enter_Q_for_quitting(string readValue)
        {
            const string expected = @"Thank you for banking with AwesomeGIC Bank.
Have a nice day!";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock(readValue);
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(2));
        }

        [Theory]
        [InlineData("Z")]
        [InlineData("")]
        public void Invalid_user_entry_prints_previous_prompt_with_indication_of_incorrect_input(string readValue)
        {
            const string expected = @"Invalid choice.
Is there anything else you'd like to do?
[T] Input transactions 
[I] Define interest rules
[P] Print statement
[Q] Quit
>";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock(readValue, "Q");
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(2));
        }

        [Theory]
        [InlineData("T")]
        [InlineData("t")]
        public void User_enter_T_for_transactions_menu(string readValue)
        {
            const string expected = @"Please enter transaction details in <Date> <Account> <Type> <Amount> format 
(or enter blank to go back to main menu):
>";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock(readValue, "Q");
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(2));
        }

        [Fact]
        public void User_enter_transaction()
        {
            const string expected = @"Account: AC001
| Date     | Txn Id      | Type | Amount |
| 20230505 | 20230505-01 | D    | 100.00 |";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock("T", "20230505 AC001 D 100.00", "Q");
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(3));
        }

        [Theory]
        [InlineData("I")]
        [InlineData("i")]
        public void User_enter_I_for_interest_rules_menu(string readValue)
        {
            const string expected = @"Please enter interest rules details in <Date> <RuleId> <Rate in %> format 
(or enter blank to go back to main menu):
>";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock(readValue, "Q");
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(2));
        }

        [Theory]
        [InlineData("P")]
        [InlineData("p")]
        public void User_enter_P_for_print_statement_menu(string readValue)
        {
            const string expected = @"Please enter account and month to generate the statement <Account> <Year><Month>
(or enter blank to go back to main menu):
>";
            var writer = StringWriter.Instance;
            var reader = new ReaderMock(readValue, "Q");
            var prompt = new Prompter(new ReadWriterMock(reader, writer));

            prompt.Launch();

            Assert.Equal(expected, writer.GetOutputNumber(2));
        }
    }
}
