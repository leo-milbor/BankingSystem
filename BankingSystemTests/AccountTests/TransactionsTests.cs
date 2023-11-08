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

            var result = t.Add(t2);

            Assert.Equal(expected, result.Value, new TransactionsEqualityComparer());
            var rest = Assert.Single(t.Value);
            Assert.Equal(new Transaction(1, new Date("20231007"), new TransactionType("D"), new Amount("100")), rest, new TransactionEqualityComparer());
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
            var actual = t.Add(t2).Add(t3);

            var filtered = actual.AtDate(new Date("20231007"));

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
            var actual = t.Add(t2).Add(t3);

            var max = actual.MaxRunningNumber();

            Assert.Equal(2, max);
        }
    }
}
