namespace BankingSystem.Account.UseCases
{
    internal interface IAccounRepository
    {
        void Add(Account account);
        Account? Get(string account);
        void Update(Account account);
    }
    internal class InputTransaction
    {
        private readonly IAccounRepository _accounRepository;

        public InputTransaction(IAccounRepository accounRepository)
        {
            _accounRepository = accounRepository;
        }

        public string Apply(string input)
        {
            var inputs = input.Split(' ');
            var date = new Date(inputs[0]);
            var accountName = inputs[1];
            var transactionType = new TransactionType(inputs[2]);
            var amount = new Amount(inputs[3]);

            var existingAccount = _accounRepository.Get(accountName);
            if (existingAccount is null)
                return AddNewAccount(accountName, date, transactionType, amount);
            else
                return UpdateAccountWithNewTransaction(existingAccount, date, transactionType, amount);
        }

        private string UpdateAccountWithNewTransaction(Account existingAccount, Date date, TransactionType transactionType, Amount amount)
        {
            existingAccount.AddTransaction(date, transactionType, amount);
            _accounRepository.Update(existingAccount);
            return existingAccount.ToString();
        }

        private string AddNewAccount(string accountName, Date date, TransactionType transactionType, Amount amount)
        {
            var transaction = new Transaction(1, date, transactionType, amount);
            var account = new Account(accountName, transaction);
            _accounRepository.Add(account);
            return account.ToString();
        }
    }
}
