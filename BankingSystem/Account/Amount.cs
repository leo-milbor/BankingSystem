using System.Globalization;

namespace BankingSystem.Account
{
    internal class Amount
    {
        private readonly decimal _amount;

        public Amount(string rawAmount)
        {
            var isDecimal = decimal.TryParse(rawAmount, out _amount);
            if (!isDecimal)
                throw new NotAValidDecimalNumberException();
            if (_amount <= 0)
                throw new NegativeAmountException();
            if (rawAmount != _amount.ToString("000.00"))
                throw new TooManyDecimalsException();
        }

        public override string ToString() => _amount.ToString(CultureInfo.CurrentCulture);
    }
    internal class NotAValidDecimalNumberException : Exception { }
    internal class TooManyDecimalsException : Exception { }
    internal class NegativeAmountException : Exception { }
}
