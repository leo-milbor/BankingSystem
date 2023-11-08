using static BankingSystem.Account.Account;
using static BankingSystem.Account.Amount;
using static BankingSystem.Account.Date;
using static BankingSystem.Account.TransactionType;

namespace BankingSystem.Account.UseCases
{
    internal interface IAccountRepository
    {
        void Add(Account account);
        Account? Get(string account);
        void Update(Account account);
    }
    internal record TransactionDTO(int RunningNumber, DateOnly Date, string Type, decimal Amount);
    internal record AccountDTO(string Id, IEnumerable<TransactionDTO> Transactions);
    internal class InputTransactionUseCase : IUseCase<AccountDTO>
    {
        private readonly IAccountRepository _accounRepository;

        public InputTransactionUseCase(IAccountRepository accounRepository)
        {
            _accounRepository = accounRepository;
        }
        public AccountDTO Apply(string input)
        {
            var inputs = input.Split(' ');
            if (inputs.Length != 4)
                throw new UseCaseException("Wrong number of argument to create an account.");
            AccountDTO dto;
            try
            {
                dto = Process(inputs);
            }
            catch (Exception e)
            {
                var message = e switch
                {
                    NotAValidDateFormatException => "Invalid date, should be in YYYYMMdd format.",
                    NotAValidTransactionTypeException => "Invalid type, D for deposit, W for withdrawal.",
                    NotAValidDecimalException => "Invalid amount, should be a correct decimal number.",
                    TooManyDecimalsException => "Invalid amount, decimals are allowed up to 2 decimal places.",
                    NegativeAmountException => "Invalid amount, must be greater than zero.",
                    EmptyTransactionsException =>"Invalid account, must have at least one transaction.",
                    NegativeBalanceException => "Invalid transaction, balance should not be less than 0.",
                    _ => "An unknown error occured."
                } ;
                throw new UseCaseException(message);
            }
            return dto;
        }

        private AccountDTO Process(string[] inputs)
        {
            string accountName = inputs[1];
            Date date = new Date(inputs[0]);
            TransactionType transactionType = new TransactionType(inputs[2]);
            Amount amount = new Amount(inputs[3]);

            var existingAccount = _accounRepository.Get(accountName);
            if (existingAccount is null)
                return AddNewAccount(accountName, date, transactionType, amount);
            else
                return UpdateAccountWithNewTransaction(existingAccount, date, transactionType, amount);
        }

        private AccountDTO UpdateAccountWithNewTransaction(Account existingAccount, Date date, TransactionType transactionType, Amount amount)
        {
            var updated = existingAccount.AddTransaction(date, transactionType, amount);
            _accounRepository.Update(updated);
            return ToDTO(updated);
        }

        private AccountDTO AddNewAccount(string accountName, Date date, TransactionType transactionType, Amount amount)
        {
            var transaction = new Transaction(1, date, transactionType, amount);
            var account = new Account(accountName, transaction);
            _accounRepository.Add(account);
            return ToDTO(account);
        }

        private static AccountDTO ToDTO(Account account)
        {
            var transactions = account.Transactions.Select(ToDTO);
            return new AccountDTO(account.Id, transactions);
        }

        private static TransactionDTO ToDTO(Transaction transaction)
        {
            return new TransactionDTO(
                transaction.RunningNumber,
                transaction.Date.Value,
                transaction.Type,
                transaction.Amount);
        }
    }
}
