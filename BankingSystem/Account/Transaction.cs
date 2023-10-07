namespace BankingSystem.Account
{
    internal class Transaction
    {
        private readonly Amount _amount;
        private TransactionType _type;
        private readonly string _id;
        public Transaction(int runningNumber, Date date, TransactionType type, Amount amount)
        {
            RunningNumber = runningNumber;
            Date = date;
            _type = type;
            _amount = amount;
            _id = $"{Date}-{RunningNumber.ToString("00")}";
        }

        public int RunningNumber { get; private set; }
        public Date Date { get; private set; }

        public override string ToString()
        {
            return $"| {Date} | {_id} | {_type}    | {_amount} |";
        }
    }
}
