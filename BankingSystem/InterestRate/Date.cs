namespace BankingSystem.InterestRule
{
    internal class Date : IEquatable<Date?>, IComparable<Date>
    {
        private readonly DateTime _date;

        public Date(string rawDate)
        {
            if (rawDate.Length != 8)
                throw new NotAValidDateFormatException();

            var isYear = int.TryParse(rawDate.Substring(0, 4), out var year);
            var isMonth = int.TryParse(rawDate.Substring(4, 2), out var month);
            var isDay = int.TryParse(rawDate.Substring(6, 2), out var day);
            if (!isYear || !isMonth || !isDay)
                throw new NotAValidDateFormatException();

            try
            {
                _date = new DateTime(year, month, day);
            }
            catch { throw new NotAValidDateFormatException(); }
        }

        public int CompareTo(Date? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return _date.CompareTo(other._date);
        }

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

        public override string ToString() => _date.ToString("yyyyMMdd");

        public static bool operator ==(Date? left, Date? right)
        {
            return EqualityComparer<Date>.Default.Equals(left, right);
        }

        public static bool operator !=(Date? left, Date? right)
        {
            return !(left == right);
        }

        internal class NotAValidDateFormatException : Exception { }
    }
}
