namespace BankingSystem
{
    internal interface IUseCase<TOut>
    {
        TOut Apply(string input);
    }
}
