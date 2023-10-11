namespace BankingSystem
{
    internal interface IFormatter<T>
    {
        string Format(T value);
    }
}
