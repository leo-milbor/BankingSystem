using System.Globalization;

namespace BankingSystem.InterestRule
{
    internal class Rate
    {
        private readonly decimal _value;
        public Rate(string rawRate)
        {
            var isDecimal = decimal.TryParse(rawRate, NumberStyles.AllowDecimalPoint, SingaporeanFormatProvider.Instance, out _value);
            if (!isDecimal)
                throw new NotAValidDecimalException();
            if (_value <= 0 ||  _value >= 100)
                throw new OutOfRangeException();
        }

        public override string ToString() => string.Format(SingaporeanFormatProvider.Instance, "{0,8:##0.00}", _value);

        internal class NotAValidDecimalException : Exception { }
        internal class OutOfRangeException : Exception { }
    }
}
