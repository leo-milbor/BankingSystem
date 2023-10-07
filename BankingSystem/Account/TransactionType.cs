namespace BankingSystem.Account
{
    internal class TransactionType
    {
        private readonly string _type;
        public TransactionType(string rawType)
        {
            _type = rawType.ToUpper() switch
            {
                "D" => "D",
                "W" => "W",
                _ => throw new NotAValidTransactionTypeException(),
            };
        }

        public override string ToString() => _type;
    }
    internal class NotAValidTransactionTypeException : Exception { }
}
