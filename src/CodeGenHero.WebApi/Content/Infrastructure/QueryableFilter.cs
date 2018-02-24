namespace CodeGenHero.WebApi
{
	public class QueryableFilter
	{
		public QueryableFilter(string member, Operators op, string value)
		{
			this.Member = member;
			this.Operator = op;
			this.Value = value;
		}

		public enum Operators
		{
			Contains,
			DoesNotContain,
			IsGreaterThan,
			IsGreaterThanOrEqual,
			IsLessThan,
			IsLessThanOrEqual,
			IsEqualTo,
			IsNotEqualTo,
			StartsWith,
			EndsWith,
			IsNull,
			IsNotNull,
			IsEmpty,
			IsNotEmpty
		}

		public string Member { get; set; }
		public Operators Operator { get; set; }
		public string Value { get; set; }
	}
}