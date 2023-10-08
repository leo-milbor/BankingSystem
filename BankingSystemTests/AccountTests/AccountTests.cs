using BankingSystem.Account;

using System.Text;

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
        public void Account_can_be_printed()
        {
            var expected = new StringBuilder()
                .Append("Account: AC001").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount |").AppendLine()
                .Append("| 20231007 | 20231007-01 | D    | 100.00 |").AppendLine()
                .Append("| 20231007 | 20231007-02 | W    |  50.00 |").ToString();

            var t1 = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var t2 = new Transaction(2, new Date("20231007"), new TransactionType("W"), new Amount("50"));
            var t = new Transactions(t1);
            t.Add(t2);
            var a = new Account("AC001", t);

            Assert.Equal(expected, a.ToString());
        }

        [Fact]
        public void Account_can_add_new_transaction()
        {
            var expected = new StringBuilder()
                .Append("Account: AC001").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount |").AppendLine()
                .Append("| 20231007 | 20231007-01 | D    | 100.00 |").AppendLine()
                .Append("| 20231007 | 20231007-02 | W    |  50.00 |").AppendLine()
                .Append("| 20231023 | 20231023-01 | D    |  25.00 |").ToString();

            var t1 = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var t = new Transactions(t1);
            var a = new Account("AC001", t);

            a.AddTransaction(new Date("20231007"), new TransactionType("W"), new Amount("50"));
            a.AddTransaction(new Date("20231023"), new TransactionType("D"), new Amount("25"));

            Assert.Equal(expected, a.ToString());
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
