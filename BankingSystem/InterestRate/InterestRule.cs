namespace BankingSystem.InterestRule
{
    internal class InterestRule : IEquatable<InterestRule?>
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
