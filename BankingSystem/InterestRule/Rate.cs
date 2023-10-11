namespace BankingSystem.InterestRule
{
    internal class Rate
    {
        private readonly decimal _value;

        public decimal Value => _value;

        public Rate(string rawRate)
        {
            try
            {
                _value = Convert.ToDecimal(rawRate, SingaporeanFormatProvider.Instance);
            }
            catch
            {
                throw new NotAValidDecimalException();
            }
            if (_value <= 0 || _value >= 100)
                throw new OutOfRangeException();
        }

        public override string ToString() => string.Format(SingaporeanFormatProvider.Instance, "{0:##0.00}", _value);

        internal class NotAValidDecimalException : Exception { }
        internal class OutOfRangeException : Exception { }
    }
}
