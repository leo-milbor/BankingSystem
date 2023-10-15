using BankingSystem.Statement.UseCases;

using System.Text;

namespace BankingSystem.Fromatter
{
    internal class PrintStatementFormatter : IFormatter<StatementDTO>
    {
        public string Format(StatementDTO value)
        {
            var transactionFormatter = new StatementTransactionFormater(6, 7);
            var sb = new StringBuilder()
                .Append($"Account: {value.Id}").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount | Balance |").AppendLine();
            foreach (var transaction in value.Transactions)
            {
                sb.Append(transactionFormatter.Format(transaction)).AppendLine();
            }
            return sb.ToString().TrimEnd();
        }
    }
    internal class StatementTransactionFormater : IFormatter<TransactionDTO>
    {
        private readonly int _amountPadding;
        private readonly int _balancePadding;

        public StatementTransactionFormater(int amountSize, int balancePadding)
        {
            _amountPadding = amountSize;
            _balancePadding = balancePadding;
        }

        public string Format(TransactionDTO value)
        {
            return $"| {FormatDate(value)} | {FormatId(value)} | {value.Type}    | {FormatAmount(value)} | {FormatBalance(value)} |";
        }

        private static string FormatDate(TransactionDTO transaction) =>
            transaction.Date.ToString("yyyyMMdd", SingaporeanFormatProvider.Instance);

        private static string FormatId(TransactionDTO transaction) =>
            transaction.Type == "I" ?
            "           " :
            $"{FormatDate(transaction)}-{transaction.RunningNumber.ToString("00")}";

        private string FormatAmount(TransactionDTO transaction) =>
            transaction.Amount.ToString("0.00", SingaporeanFormatProvider.Instance).PadLeft(_amountPadding);

        private string FormatBalance(TransactionDTO transaction) =>
            transaction.Balance.ToString("0.00", SingaporeanFormatProvider.Instance).PadLeft(_balancePadding);
    }
}
