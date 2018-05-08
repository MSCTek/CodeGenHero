using System.Collections.Generic;

namespace CodeGenHero.DataService
{
	public interface IPageDataRequest
	{
		List<IFilterCriterion> FilterCriteria { get; set; }
		int Page { get; set; }
		int PageSize { get; set; }
		string Sort { get; set; }
	}
}