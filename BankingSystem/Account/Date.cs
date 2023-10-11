using System.Globalization;

namespace BankingSystem.Account
{
    internal class Date : IComparable<Date>
    {
        private const string _format = "yyyyMMdd";
        private readonly DateOnly _date;

        public DateOnly Value => _date;

        public Date(string rawDate)
        {
            // DateOnly.TryParseExact won't work as expected, so I resorted to DateTime for parsing.
            var isCorrectDate = DateTime.TryParseExact(rawDate, _format, SingaporeanFormatProvider.Instance, DateTimeStyles.AssumeLocal, out var date);
            if (!isCorrectDate)
                throw new NotAValidDateFormatException();
            _date = new DateOnly(date.Year, date.Month, date.Day);
        }

        public int CompareTo(Date? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return _date.CompareTo(other._date);
        }

        internal class NotAValidDateFormatException : Exception { }
    }
}
