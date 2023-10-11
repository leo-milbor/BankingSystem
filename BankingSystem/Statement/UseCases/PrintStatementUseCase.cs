namespace BankingSystem.Statement.UseCases
{
    internal record StatementDTO();
    internal class PrintStatementUseCase : IUseCase<StatementDTO>
    {
        public StatementDTO Apply(string input)
        {
            throw new NotImplementedException();
        }
    }
}
