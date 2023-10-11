namespace BankingSystem.InterestRule
{
    internal class InterestRule : IEquatable<InterestRule?>
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

        public override string ToString() => $"| {_date} | {_id} | {_rate} |";

        #region Equality
        public override bool Equals(object? obj)
        {
            return Equals(obj as InterestRule);
        }

        public bool Equals(InterestRule? other)
        {
            return other is not null &&
                   _id == other._id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }

        public static bool operator ==(InterestRule? left, InterestRule? right)
        {
            return EqualityComparer<InterestRule>.Default.Equals(left, right);
        }

        public static bool operator !=(InterestRule? left, InterestRule? right)
        {
            return !(left == right);
        }
        #endregion Equality
    }
}
