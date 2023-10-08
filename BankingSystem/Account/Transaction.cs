namespace BankingSystem.Account
{
    internal class Transaction
    {
        private readonly Amount _amount;
        private readonly TransactionType _type;
        private readonly string _id;
        private readonly int _runningNumber;
        private readonly Date _date;

        public int RunningNumber => _runningNumber;
        public Date Date => _date;
        public bool IsWithdrawal => _type.IsWithdrawal;
        public decimal Amount => IsWithdrawal ? -_amount.Value : _amount.Value;

        public Transaction(int runningNumber, Date date, TransactionType type, Amount amount)
        {
            _runningNumber = runningNumber;
            _date = date;
            _type = type;
            _amount = amount;
            _id = $"{_date}-{_runningNumber.ToString("00")}";
        }

        public override string ToString()
        {
            return $"| {Date} | {_id} | {_type} | {_amount} |";
        }
    }
}
