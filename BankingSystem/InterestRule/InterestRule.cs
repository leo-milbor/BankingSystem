namespace BankingSystem.InterestRule
{
    internal class InterestRule
    {
        private readonly string _id;
        private readonly Date _date;
        private readonly Rate _rate;

        public decimal Rate => _rate.Value;
        public DateOnly Date => _date.Value;
        public string Id => _id;

        public InterestRule(string id, Date date, Rate rate)
        {
            _id = id;
            _date = date;
            _rate = rate;
        }
    }
}
