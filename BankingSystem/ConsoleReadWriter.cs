namespace BankingSystem
{
    internal class ConsoleReadWriter : IReadWriter    {
        public static ConsoleReadWriter Instance => new ConsoleReadWriter();

        public string Read()
        {
            return Console.ReadLine() ?? "";
        }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
