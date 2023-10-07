using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

using Microsoft.VisualBasic;

namespace BankingSystem.Account
{
    internal class Date
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

        public override string ToString() => _date.ToString("yyyyMMdd");
    }
    internal class NotAValidDateFormatException : Exception { }
}
