using System.Globalization;

namespace BankingSystem.InterestRule
{
    internal class Date
    {
        private const string _format = "yyyyMMdd";
        private readonly DateOnly _date;

        public DateOnly Value => _date;

        public Date(string rawDate)
        {
            var isCorrectDate = DateTime.TryParseExact(rawDate, _format, SingaporeanFormatProvider.Instance, DateTimeStyles.AssumeLocal, out var date);
            if (!isCorrectDate)
                throw new NotAValidDateFormatException();
            _date = new DateOnly(date.Year, date.Month, date.Day);
        }

        internal class NotAValidDateFormatException : Exception { }
    }
}
