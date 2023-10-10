using System.Globalization;

namespace BankingSystem.Account
{
    internal class Date : IEquatable<Date?>, IComparable<Date>
    {
        private const string _format = "yyyyMMdd";
        private readonly DateTime _date;

        public Date(string rawDate)
        {
            var isCorrectDate = DateTime.TryParseExact(rawDate, _format, SingaporeanFormatProvider.Instance, DateTimeStyles.AssumeLocal, out _date);
            if (!isCorrectDate)
                throw new NotAValidDateFormatException();
        }

        public override string ToString() => _date.ToString(_format, SingaporeanFormatProvider.Instance);

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
