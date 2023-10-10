namespace BankingSystem.InterestRule
{
    internal class InterestRule
    {
        private string _id;
        private Date _date;
        private Rate _rate;

        public InterestRule(string id, Date date, Rate rate)
        {
            _id = id;
            _date = date;
            _rate = rate;
        }

        public override string ToString() => $"| {_date} | {_id} | {_rate} |";
    }
}
