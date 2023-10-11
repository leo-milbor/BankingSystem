namespace BankingSystem.Account
{
    internal class TransactionType
    {
        private readonly char _type;
        public TransactionType(string rawType)
        {
            _type = rawType.ToUpper() switch
            {
                "D" => 'D',
                "W" => 'W',
                _ => throw new NotAValidTransactionTypeException(),
            };
        }

        public bool IsWithdrawal => _type == 'W';

        public string Value => _type.ToString();

        internal class NotAValidTransactionTypeException : Exception { }
    }
}
