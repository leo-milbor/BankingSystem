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
            if (existingAccount == null)
            {
                var transaction = new Transaction(1, date, transactionType, amount);
                var account = new Account(accountName, new Transactions(transaction));
                _accounRepository.Add(account);
                existingAccount = account;
            }
            else
            {
                existingAccount.Add(date, transactionType, amount);
                _accounRepository.Update(existingAccount);
            }

            return existingAccount.ToString();
        }
    }
}
