namespace CodeGenHero.EAMVCXamPOCO
{
	public class PageData<T> : PageData
	{
		public PageData() : base()
		{
		}

		public PageData(T data) : base()
		{
			Data = data;
		}

		public T Data { get; set; }
	}

	public class PageData
	{
		public PageData()
		{
		}

		public PageData(int currentPage, bool isSuccessStatusCode, string nextPageLink, int pageSize, string previousPageLink, int totalCount, int totalPages)
		{
			CurrentPage = currentPage;
			IsSuccessStatusCode = isSuccessStatusCode;
			NextPageLink = nextPageLink;
			PageSize = pageSize;
			PreviousPageLink = previousPageLink;
			TotalCount = totalCount;
			TotalPages = totalPages;
		}

		public int CurrentPage { get; set; }
		public bool IsSuccessStatusCode { get; set; }
		public string NextPageLink { get; set; }
		public int PageSize { get; set; }
		public string PreviousPageLink { get; set; }
		public int TotalCount { get; set; }
		public int TotalPages { get; set; }
	}
}