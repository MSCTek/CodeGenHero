using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections;

namespace CodeGenHero.WebApi
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, QueryableFilter filter)
		{
			var propertyInfoMatch = typeof(T).GetProperty(filter.Member, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (propertyInfoMatch == null)
				throw new ArgumentException($"Unexpected filter field of {filter.Member} does not match a property on object {typeof(T).ToString()}");

			bool isCollection = propertyInfoMatch.PropertyType != typeof(string) &&
				typeof(IEnumerable).IsAssignableFrom(propertyInfoMatch.PropertyType);
			string fullName = propertyInfoMatch.PropertyType.FullName;
			bool isDateTime = fullName.Contains("System.DateTime");  // Contains handles nullable fields.
			bool isGuid = fullName.Contains("System.Guid");  // Contains handles nullable fields.
			bool isBoolean = fullName.Contains("System.Boolean");  // Contains handles nullable fields.
			bool isShort = fullName.Contains("System.Int16");

			Func<string, string, string> condition;

			if (isCollection)
				condition = (format, member) => string.Format("{0}.Any({1})", member, string.Format(format, "it")); // System.Linq.Dynamic Collection:  {PropertyName}.Any(it{condition})
			else
				condition = (format, member) => string.Format(format, member); // System.Linq.Dynamic Object:  {PropertyName}{condition}

			object typedFilterValue = filter.Value;
			if (isDateTime)
				typedFilterValue = DateTime.Parse(filter.Value);
			else if (isGuid)
				typedFilterValue = Guid.Parse(filter.Value);
			else if (isBoolean)
				typedFilterValue = Boolean.Parse(filter.Value);
			else if (isShort)
				typedFilterValue = Int16.Parse(filter.Value);

			switch (filter.Operator)
			{
				case QueryableFilter.Operators.IsGreaterThanOrEqual:
					source = source.Where(condition("{0} >= (@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsLessThanOrEqual:
					source = source.Where(condition("{0} <= (@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsGreaterThan:
					source = source.Where(condition("{0} > (@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsLessThan:
					source = source.Where(condition("{0} < (@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.Contains:
					source = source.Where(condition("{0}.Contains(@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.DoesNotContain:
					source = source.Where(condition("!{0}.Contains(@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsEqualTo:
					source = source.Where(condition("{0} = @0", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsNotEqualTo:
					source = source.Where(condition("{0} != @0", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.StartsWith:
					source = source.Where(condition("{0}.StartsWith(@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.EndsWith:
					source = source.Where(condition("{0}.EndsWith(@0)", filter.Member), typedFilterValue);
					break;

				case QueryableFilter.Operators.IsNull:
					source = source.Where(condition("{0} = NULL", filter.Member));
					break;

				case QueryableFilter.Operators.IsNotNull:
					source = source.Where(condition("{0} != NULL", filter.Member));
					break;

				case QueryableFilter.Operators.IsEmpty:
					source = source.Where(condition("string.IsNullOrEmpty({0})", filter.Member));
					break;

				case QueryableFilter.Operators.IsNotEmpty:
					source = source.Where(condition("!string.IsNullOrEmpty({0})", filter.Member));
					break;
			}

			return source;
		}

		public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, System.Collections.Generic.List<string> filterList)
		{
			filterList.ForEach(x =>
			{
				var indexOfFirstDelimiter = x.IndexOf("~");
				string filterField = x.Substring(0, indexOfFirstDelimiter);
				var indexOfSecondDelimiter = x.IndexOf("~", indexOfFirstDelimiter + 1);
				string operandString = x.Substring(indexOfFirstDelimiter + 1, indexOfSecondDelimiter - indexOfFirstDelimiter - 1);
				string filterValue = x.Substring(indexOfSecondDelimiter + 1, x.Length - indexOfSecondDelimiter - 1);
				var op = (QueryableFilter.Operators)Enum.Parse(typeof(QueryableFilter.Operators), operandString, true);

				source = source.ApplyFilter(new QueryableFilter(filterField, op, filterValue)); //.Where(x => x.UpdatedDate >= updatedDate);
			});

			return source;
		}

		public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (sort == null)
			{
				return source;
			}

			// split the sort string
			var lstSort = sort.Split(',');

			// run through the sorting options and create a sort expression string from them

			string completeSortExpression = "";
			foreach (var sortOption in lstSort)
			{
				// if the sort option starts with "-", we order
				// descending, otherwise ascending

				if (sortOption.StartsWith("-"))
				{
					completeSortExpression = completeSortExpression + sortOption.Remove(0, 1) + " descending,";
				}
				else
				{
					completeSortExpression = completeSortExpression + sortOption + ",";
				}
			}

			if (!string.IsNullOrWhiteSpace(completeSortExpression))
			{
				source = source.OrderBy(completeSortExpression.Remove(completeSortExpression.Count() - 1));
			}

			return source;
		}
	}
}