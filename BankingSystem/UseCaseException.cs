namespace BankingSystem
{
    internal class UseCaseException : Exception
    {
        public UseCaseException(string message) : base(message) { }

        public static UseCaseException UnknownError => new UseCaseException("An unknown error occured.");
    }
}
