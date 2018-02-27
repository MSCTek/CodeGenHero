namespace CodeGenHero.DataService
{
	public interface IPageData
	{
		int CurrentPage { get; set; }
		bool IsSuccessStatusCode { get; set; }
		string NextPageLink { get; set; }
		int PageSize { get; set; }
		string PreviousPageLink { get; set; }
		int TotalCount { get; set; }
		int TotalPages { get; set; }
	}
}