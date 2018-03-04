//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using entCCO = MSC.BingoBuzz.Repository.Entities.CCO;

//namespace MSC.BingoBuzz.API.BB
//{
//	public partial class TestBBController : BBBaseApiController
//	{
//		/// <summary>
//		/// Custom logic used to filter on a field that exists in a related, parent, table.
//		/// </summary>
//		/// <param name="dbItems"></param>
//		/// <param name="filterList"></param>
//		partial void RunCustomLogicAfterGetQueryableList(ref IQueryable<entCCO.css_Answer> dbItems, ref List<string> filterList)
//		{
//			dbItems = dbItems.Include(x => x.foo);

//			// Just a sample of how to use this injection point:
//			//var queryableFilters = filterList.ToQueryableFilter();
//			//var customCriterion = queryableFilters.Where(y => y.Member.ToLowerInvariant() == "childquestionindicator").FirstOrDefault();

//			//if (customCriterion != null)
//			//{
//			//	if (!bool.TryParse(customCriterion.Value, out var barCriterionValue))
//			//		throw new ArgumentException($"Invalid value specified for ChildQuestionIndicator parameter.");

//			//	dbItems = dbItems.Where(x => x.foo.bar == barCriterionValue);
//			//	queryableFilters.Remove(customCriterion);  // The evaluated criterion needs to be removed from the list of filters before we invoke the ApplyFilter() extension method.
//			//	filterList = queryableFilters.ToQueryableStringList();
//			//}
//		}
//	}
//}