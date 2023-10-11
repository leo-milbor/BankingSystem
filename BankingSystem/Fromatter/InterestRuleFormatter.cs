using System.Text;

using BankingSystem.InterestRule.UseCases;

namespace BankingSystem.Fromatter
{
    internal class InterestRulesFormatter : IFormatter<IEnumerable<InterestRuleDTO>>
    {
        public static InterestRulesFormatter Instance => new InterestRulesFormatter();

        public string Format(IEnumerable<InterestRuleDTO> value)
        {
            var ruleFormatter = new InterestRuleFormatter(8);
            var sb = new StringBuilder()
                .Append("Interest rules:").AppendLine()
                .Append("| Date     | RuleId | Rate (%) |").AppendLine();
            foreach (var rule in value)
            {
                sb.Append(ruleFormatter.Format(rule)).AppendLine();
            }
            return sb.ToString().TrimEnd();
        }
    }

    internal class InterestRuleFormatter : IFormatter<InterestRuleDTO>
    {
        private readonly int _ratePadding;

        public InterestRuleFormatter(int ratePadding)
        {
            _ratePadding = ratePadding;
        }

        public string Format(InterestRuleDTO value)
        {
            return $"| {FormatDate(value)} | {value.Id} | {FormatRate(value)} |";
        }

        private static string FormatDate(InterestRuleDTO rule) =>
            rule.Date.ToString("yyyyMMdd", SingaporeanFormatProvider.Instance);

        private string FormatRate(InterestRuleDTO rule) =>
            rule.Rate.ToString("0.00").PadLeft(_ratePadding);
    }
}
