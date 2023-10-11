using System.Globalization;

namespace BankingSystem.Account
{
    internal class Date : IEquatable<Date?>, IComparable<Date>
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

        #region Equality and comparison
        public override bool Equals(object? obj)
        {
            return Equals(obj as Date);
        }

        public bool Equals(Date? other)
        {
            return other is not null &&
                   _date == other._date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_date);
        }

        public static bool operator ==(Date? left, Date? right)
        {
            return EqualityComparer<Date>.Default.Equals(left, right);
        }

        public static bool operator !=(Date? left, Date? right)
        {
            return !(left == right);
        }

        public int CompareTo(Date? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return _date.CompareTo(other._date);
        }

        #endregion Equality and comparison

        internal class NotAValidDateFormatException : Exception { }
    }
}
