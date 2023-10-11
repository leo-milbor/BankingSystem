namespace BankingSystem
{
    internal class UseCaseException : Exception
    {
        public string Message { get; }

        public UseCaseException(string message)
        {
            Message = message;
        }

        public static UseCaseException UnknownError => new UseCaseException("An unknown error occured.");
    }
}
