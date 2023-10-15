using System.Globalization;

using BankingSystem.Statement.UseCases;
using AccountUS = BankingSystem.Account;

namespace BankingSystem.Statement.Repository
{
    internal class InMemoryAccountRepository : IAccountRepository
    {
        private readonly AccountUS.UseCases.IAccountRepository _accountRepository;

        public InMemoryAccountRepository(AccountUS.UseCases.IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account? Get(string Id)
        {
            var account = _accountRepository.Get(Id);
            if (account is null) return null;
            return new Account(
                Id, account.Transactions.Select(
                    t => new Transaction(
                        FormatId(t.Date.Value, t.RunningNumber),
                        t.Date.Value,
                        t.RunningNumber,
                        t.Type,
                        t.Amount,
                        GetBalance(t, account.Transactions))));
        }

        private static string FormatId(DateOnly date, int runningNumber) =>
            $"{date}-{runningNumber.ToString("00", CultureInfo.InvariantCulture)}";

        private static decimal GetBalance(AccountUS.Transaction transaction, IEnumerable<AccountUS.Transaction> transactions) => 
            transactions
                .Where(t => t.Date.Value <= transaction.Date.Value
                                 && t.RunningNumber < transaction.RunningNumber)
                .Sum(t => t.Amount);
    }
}
