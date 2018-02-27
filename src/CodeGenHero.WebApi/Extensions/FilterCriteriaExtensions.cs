using System;
using System.Collections.Generic;

namespace CodeGenHero.WebApi
{
	public static class FilterCriteriaExtensions
	{
		public static List<QueryableFilter> ToQueryableFilter(this List<string> strList)
		{
			List<QueryableFilter> retVal = new List<QueryableFilter>();

			strList.ForEach(x =>
			{
				var indexOfFirstDelimiter = x.IndexOf("~");
				string filterField = x.Substring(0, indexOfFirstDelimiter);
				var indexOfSecondDelimiter = x.IndexOf("~", indexOfFirstDelimiter + 1);
				string operandString = x.Substring(indexOfFirstDelimiter + 1, indexOfSecondDelimiter - indexOfFirstDelimiter - 1);
				string filterValue = x.Substring(indexOfSecondDelimiter + 1, x.Length - indexOfSecondDelimiter - 1);
				var op = (QueryableFilter.Operators)Enum.Parse(typeof(QueryableFilter.Operators), operandString, true);

				var newQF = new QueryableFilter(filterField, op, filterValue);

				retVal.Add(newQF);
			});

			return retVal;
		}

		public static List<string> ToQueryableStringList(this List<QueryableFilter> queryableFilters)
		{
			var retVal = new List<string>();

			queryableFilters.ForEach(x =>
			{
				var str = $"{x.Member}~{Enum.GetName(typeof(QueryableFilter.Operators), x.Operator)}~{x.Value}";
				retVal.Add(str);
			});

			return retVal;
		}
	}
}