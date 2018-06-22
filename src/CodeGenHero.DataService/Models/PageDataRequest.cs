using System.Collections.Generic;

namespace CodeGenHero.DataService
{
	public class PageDataRequest : IPageDataRequest
	{
		public PageDataRequest()
		{
			this.FilterCriteria = new List<IFilterCriterion>();
			this.Sort = null;
			this.Page = 1;
			this.PageSize = 100;
		}

		public PageDataRequest(IList<IFilterCriterion> filterCriteria, string sort = null, int page = 1, int pageSize = 100)
		{
			this.FilterCriteria = filterCriteria;
			this.Sort = sort;
			this.Page = page;
			this.PageSize = pageSize;
		}

		public IList<IFilterCriterion> FilterCriteria { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string Sort { get; set; }
	}
}