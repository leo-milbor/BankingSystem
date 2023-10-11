using BankingSystem.Account;

namespace BankingSystemTests.AccountTests
{
    public class TransactionsTests
    {
        [Fact]
        public void Transactions_can_be_added()
        {
            var expected = new List<Transaction>()
            {
                new Transaction(1, new Date("20231007"),new TransactionType("D"), new Amount("100")),
                new Transaction(2, new Date("20231007"),new TransactionType("W"), new Amount("50"))
            };
            var t1 = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var t2 = new Transaction(2, new Date("20231007"), new TransactionType("W"), new Amount("50"));
            var t = new Transactions(t1);
            t.Add(t2);

            Assert.Equal(expected, t.Value, new TransactionsEqualityComparer());
        }

        [Fact]
        public void Transactions_can_be_filtered_by_date()
        {
            var expected = new List<Transaction>()
            {
                new Transaction(1, new Date("20231007"),new TransactionType("D"), new Amount("100")),
                new Transaction(2, new Date("20231007"),new TransactionType("W"), new Amount("50"))
            };
            var t1 = new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100"));
            var t2 = new Transaction(2, new Date("20231007"), new TransactionType("W"), new Amount("50"));
            var t3 = new Transaction(1, new Date("20231014"), new TransactionType("D"), new Amount("150"));
            var t = new Transactions(t1);
            t.Add(t2);
            t.Add(t3);

            var filtered = t.AtDate(new Date("20231007"));

            Assert.Equal(expected, filtered.Value, new TransactionsEqualityComparer());
        }

        [Fact]
        public void Transactions_can_return_highest_running_number()
        {
            var theDate = new Date("20231007");
            var t1 = new Transaction(1, theDate, new TransactionType("D"), new Amount("100"));
            var t2 = new Transaction(2, theDate, new TransactionType("W"), new Amount("50"));
            var t3 = new Transaction(1, new Date("20231014"), new TransactionType("D"), new Amount("150"));
            var t = new Transactions(t1);
            t.Add(t2);
            t.Add(t3);

            var max = t.MaxRunningNumber();

            Assert.Equal(2, max);
        }
    }
}
