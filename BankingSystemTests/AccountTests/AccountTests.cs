using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class AccountTests
    {
        [Fact]
        public void Transactions_on_account_cannot_be_empty()
        {
            var t = new Transaction(
                1, new Date("20231225"),
                new TransactionType("D"),
                new Amount("1"));
            var emptyTransactions = new Transactions(t)
                .AtDate(new Date("20221225"));

            Assert.Throws<Account.EmptyTransactionsException>(
                () => new Account("Some account", emptyTransactions));
        }

        [Fact]
        public void Account_can_add_new_transaction()
        {
            var expectedTransactions = new List<Transaction>()
            {
                new Transaction(1, new Date("20231007"),new TransactionType("D"), new Amount("100")),
                new Transaction(2, new Date("20231007"),new TransactionType("W"), new Amount("50")),
                new Transaction(1, new Date("20231023"),new TransactionType("D"), new Amount("25"))
            };

            var t1 = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var t = new Transactions(t1);
            var a = new Account("AC001", t);

            var actual = a.AddTransaction(new Date("20231007"), new TransactionType("W"), new Amount("50"))
             .AddTransaction(new Date("20231023"), new TransactionType("D"), new Amount("25"));

            Assert.Equal("AC001", actual.Id);
            Assert.Equal(expectedTransactions, actual.Transactions.ToList(), new TransactionsEqualityComparer());
        }

        [Fact]
        public void Account_cannot_be_created_with_withdrawal()
        {
            var t = new Transaction(1, new Date("20231007"), new TransactionType("W"), new Amount("100"));
            Assert.Throws<Account.NegativeBalanceException>(() => new Account("AC001", t));
        }

        [Fact]
        public void Account_cannot_have_negative_balance()
        {
            var t = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var a = new Account("AC001", t);
            Assert.Throws<Account.NegativeBalanceException>(
                () => a.AddTransaction(new Date("20231010"), new TransactionType("W"), new Amount("150")));
        }
    }
}
