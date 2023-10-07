using System.Globalization;

namespace BankingSystem.Account
{
    internal class Amount
    {
        private static readonly CultureInfo _cultureInfo = new("en-US");
        private readonly decimal _amount;

        public decimal Value => _amount;

        public Amount(string rawAmount)
        {
            try
            {
                _amount = Convert.ToDecimal(rawAmount, _cultureInfo);
            }
            catch
            {
                throw new NotAValidDecimalException();
            }
            if (_amount <= 0)
                throw new NegativeAmountException();
            if (rawAmount.Length > Format(_amount).TrimStart().Length)
                throw new TooManyDecimalsException();
        }

        public override string ToString() => Format(_amount);

        private static string Format(decimal amount) => string.Format(_cultureInfo, "{0,6:##0.00}", amount);
        internal class NotAValidDecimalException : Exception { }
        internal class TooManyDecimalsException : Exception { }
        internal class NegativeAmountException : Exception { }
    }
}
