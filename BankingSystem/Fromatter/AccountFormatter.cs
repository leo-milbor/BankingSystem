﻿using System.Text;

using BankingSystem.Account.UseCases;

namespace BankingSystem.Fromatter
{
    internal class AccountFormatter : IFormatter<AccountDTO>
    {
        public string Format(AccountDTO value)
        {
            var transactionFormatter = new TransactionFormater(6);
            var sb = new StringBuilder()
                .Append($"Account: {value.Id}").AppendLine()
                .Append("| Date     | Txn Id      | Type | Amount |").AppendLine();
            foreach (var transaction in value.Transactions)
            {
                sb.Append(transactionFormatter.Format(transaction)).AppendLine();
            }
            return sb.ToString().TrimEnd();
        }
    }
    internal class TransactionFormater : IFormatter<TransactionDTO>
    {
        private readonly int _amountPadding;

        public TransactionFormater(int amountSize)
        {
            _amountPadding = amountSize;
        }

        public string Format(TransactionDTO value)
        {
            return $"| {FormatDate(value)} | {FormatId(value)} | {value.Type}    | {FormatAmount(value)} |";
        }

        private static string FormatDate(TransactionDTO transaction) =>
            transaction.Date.ToString("yyyyMMdd", SingaporeanFormatProvider.Instance);

        private static string FormatId(TransactionDTO transaction) =>
            $"{FormatDate(transaction)}-{transaction.RunningNumber.ToString("00")}";

        private string FormatAmount(TransactionDTO transaction) =>
            transaction.Amount.ToString("0.00", SingaporeanFormatProvider.Instance).PadLeft(_amountPadding);
    }
}
