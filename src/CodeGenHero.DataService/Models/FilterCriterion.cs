using System;
using static CodeGenHero.DataService.Enums;

namespace CodeGenHero.DataService
{
	public class FilterCriterion : IFilterCriterion
	{
		public string FieldType { get; set; }
		public string FieldName { get; set; }
		public string FilterOperator { get; set; }  // ApiFilterOperator 
		public object Value { get; set; }
	}
}