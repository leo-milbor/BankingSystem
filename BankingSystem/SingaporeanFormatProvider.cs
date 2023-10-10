using System.Globalization;

namespace BankingSystem
{
    internal static class SingaporeanFormatProvider
    {
        public static IFormatProvider Instance => new CultureInfo("en-SG");
    }
}
