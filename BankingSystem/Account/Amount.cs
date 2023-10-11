namespace BankingSystem.Account
{
    internal class Amount
    {
        private readonly decimal _value;

        public decimal Value => _value;

        public Amount(string rawAmount)
        {
            try
            {
                _value = Convert.ToDecimal(rawAmount, SingaporeanFormatProvider.Instance);
            }
            catch
            {
                throw new NotAValidDecimalException();
            }
            if (_value <= 0)
                throw new NegativeAmountException();
            if (rawAmount.Length > Format(_value).TrimStart().Length)
                throw new TooManyDecimalsException();
        }

        private static string Format(decimal amount) => string.Format(SingaporeanFormatProvider.Instance, "{0:##0.00}", amount);
        internal class NotAValidDecimalException : Exception { }
        internal class TooManyDecimalsException : Exception { }
        internal class NegativeAmountException : Exception { }
    }
}
