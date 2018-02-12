using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CodeGenHero.EAMVCXamPOCO.Interface;
using CodeGenHero.EAMVCXamPOCO.DataService.Interface;
using CGHEnums = CodeGenHero.EAMVCXamPOCO.Enums;
using CGHConstants = CodeGenHero.EAMVCXamPOCO.Constants;

namespace CodeGenHero.EAMVCXamPOCO.DataService
{
	public abstract class WebApiDataServiceBase : IWebApiDataServiceBase
	{
		#region Constructor and Context

		//private PublicClientApplication _publicClientApplication;
		//private TokenCache _tokenCache;

		public WebApiDataServiceBase(ILoggingService log, IWebApiExecutionContext context)
		{
			Log = log;
			ExecutionContext = context;
		}

		protected WebApiDataServiceBase()
		{
		}

		public AuthenticationHeaderValue DefaultAuthenticationHeaderValue { get; set; }

		public string DefaultConnectionIdentifier
		{
			get
			{
				string retVal = this.ExecutionContext?.ConnectionIdentifier;
				return retVal;
			}
		}

		public int DefaultRequestedVersion { get; set; }
		public IWebApiExecutionContext ExecutionContext { get; set; }

		public ILoggingService Log { get; set; }
		//public PublicClientApplication PublicClientApplication
		//{
		//    get
		//    {
		//        return _publicClientApplication;
		//    }
		//    set
		//    {
		//        _publicClientApplication = value;
		//    }
		//}

		//public TokenCache TokenCache
		//{
		//    get
		//    {
		//        return _tokenCache;
		//    }
		//    set
		//    {
		//        _tokenCache = value;
		//    }
		//}

		#endregion Constructor and Context

		#region Client Code

		//public HttpClient GetClient(string requestedVersion = "1")
		//{
		//    AuthenticationHeaderValue authHeaderValue = null;
		//    //if (ExecutionContext.AuthenticationHeaderValueType == MMSEnums.WebApiAuthenticationHeaderValueType.BearerToken
		//    //    && !string.IsNullOrEmpty(ExecutionContext?.ClientSecret))
		//    //{
		//    //    ClientCredential credential = new ClientCredential(ExecutionContext.ClientSecret);
		//    //    var app = new ConfidentialClientApplication(ExecutionContext.ClientId, ExecutionContext.RedirectUri, credential, this.TokenCache) { };
		//    //    authHeaderValue = ExecutionContext.GetAuthorizationOAuth(app);
		//    //}
		//    //else if
		//    if (ExecutionContext.AuthenticationHeaderValueType == MMSEnums.WebApiAuthenticationHeaderValueType.BearerToken
		//                   && this.PublicClientApplication != null)
		//    {
		//        authHeaderValue = ExecutionContext.GetAuthorizationOAuth(this.PublicClientApplication);
		//    }
		//    else if (ExecutionContext.AuthenticationHeaderValueType == MMSEnums.WebApiAuthenticationHeaderValueType.BearerToken
		//    && this.TokenCache != null)
		//    {
		//        authHeaderValue = ExecutionContext.GetAuthorizationOAuth(this.TokenCache);
		//    }

		//    HttpClient retVal = GetClient(authHeaderValue, requestedVersion);
		//    return retVal;
		//}

		public HttpClient GetClient(int requestedVersion = 1, string connectionIdentifier = null)
		{
			return GetClient(DefaultAuthenticationHeaderValue, requestedVersion, DefaultConnectionIdentifier);
		}

		public HttpClient GetClient(AuthenticationHeaderValue authorization, int requestedVersion, string connectionIdentifier)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(ExecutionContext.BaseWebApiUrl);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (requestedVersion > 0)
			{
				// Using a custom request header
				client.DefaultRequestHeaders.Add("api-version", requestedVersion.ToString());

				// Using content negotiation
				//client.DefaultRequestHeaders.Accept.Add(
				//    new MediaTypeWithQualityHeaderValue("application/vnd.mmsapi.v"
				//        + requestedVersion + "+json"));
			}

			if (!string.IsNullOrEmpty(connectionIdentifier))
			{
				client.DefaultRequestHeaders.Add(CGHConstants.CONNECTIONIDENTIFIER, connectionIdentifier);
			}

			if (authorization != null)
			{
				client.DefaultRequestHeaders.Authorization = authorization;
			}

			return client;
		}

		#endregion Client Code

		#region Convenience Methods

		protected List<string> BuildFilter(Dictionary<string, string> filters, DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			var retVal = new List<string>();

			if (filters != null)
			{
				foreach (KeyValuePair<string, string> pair in filters)
				{
					retVal.Add($"{pair.Key}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{pair.Value}");
				}
			}

			if (minUpdatedDate.HasValue && !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilter(Guid? companyId, DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			return BuildFilter(companyId, null, minUpdatedDate, updatedDateFieldName);
		}

		protected List<string> BuildFilter(Guid? companyId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (companyId.HasValue && companyId != Guid.Empty)
			{
				retVal.Add($"companyId{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{companyId}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilter(bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilter(short? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilter(DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			return BuildFilter(Guid.Empty, minUpdatedDate, updatedDateFieldName);
		}

		protected List<string> BuildFilterForCompanyCatalog(Guid companyCatalogId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			retVal.Add($"companyCatalogId{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{companyCatalogId}");

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilterForProductId(Guid productId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			retVal.Add($"productId{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{productId}");

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected List<string> BuildFilterForPurchaseOrderLine(Guid puchaseOrderRevisionId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			retVal.Add($"PurchaseOrderRevisionId{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISEQUALTO}{CGHConstants.API_FILTER_DELIMITER}{puchaseOrderRevisionId}");

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{CGHConstants.API_FILTER_DELIMITER}{CGHConstants.OPERATOR_ISGREATERTHANOREQUAL}{CGHConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<int, int,
			Task<PageData<List<T>>>> getMethodToRun)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} pageSize = {pageSize}.",

					CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<DateTime?, int, int,
			Task<PageData<List<T>>>> getMethodToRun, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using minUpdatedDate = {dateString}",
						CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<bool?, DateTime?, int, int,
			Task<PageData<List<T>>>> getMethodToRun, bool? isDeleted, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(isDeleted, minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using isDeleted {isDeleted} and minUpdatedDate = {dateString}",
						CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<short?, DateTime?, int, int,
			Task<PageData<List<T>>>> getMethodToRun, short? isDeleted, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(isDeleted, minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using isDeleted {isDeleted} and minUpdatedDate = {dateString}",
						CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		//protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<Guid?, DateTime?, int, int,
		//    Task<PageData<List<T>>>> getMethodToRun, Guid? companyId, DateTime? minUpdatedDate)
		//{
		//    List<T> retVal = new List<T>();
		//    PageData<List<T>> results = null;
		//    int currentPage = 1;
		//    int pageSize = 100;

		//    while (results == null
		//        || (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
		//    {
		//        results = await getMethodToRun(companyId, minUpdatedDate, currentPage, pageSize);
		//        if (results.IsSuccessStatusCode)
		//        {
		//            retVal.AddRange(results.Data);
		//        }
		//        else
		//        {
		//            string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
		//            Context.Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using companyId {companyId} and minUpdatedDate = {dateString}",
		//                MMSEnums.LogMessageType.Exception_WebApi);
		//        }

		//        currentPage++;
		//    }

		//    return retVal;
		//}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<Guid, bool?, DateTime?, int, int,
			Task<PageData<List<T>>>> getMethodToRun, Guid idCriterion, bool? isDeleted, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(idCriterion, isDeleted, minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using companyId {idCriterion} isDeleted = {isDeleted} and minUpdatedDate = {dateString}",
						Enums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<Guid?, bool?, DateTime?, int, int,
		Task<PageData<List<T>>>> getMethodToRun, Guid? idCriterion, bool? isDeleted, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(idCriterion, isDeleted, minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using companyId {idCriterion} isDeleted = {isDeleted} and minUpdatedDate = {dateString}",
						CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<int, Guid?, bool?, DateTime?, int, int,
			Task<PageData<List<T>>>> getMethodToRun, int id, Guid? companyId, bool? isDeleted, DateTime? minUpdatedDate)
		{
			List<T> retVal = new List<T>();
			PageData<List<T>> results = null;
			int currentPage = 1;
			int pageSize = 100;

			while (results == null
				|| (results.IsSuccessStatusCode == true && currentPage <= results.TotalPages))
			{
				results = await getMethodToRun(id, companyId, isDeleted, minUpdatedDate, currentPage, pageSize);
				if (results.IsSuccessStatusCode)
				{
					retVal.AddRange(results.Data);
				}
				else
				{
					string dateString = minUpdatedDate.HasValue ? minUpdatedDate.Value.ToString() : string.Empty;
					Log.Error($"Failure detected during data retrieval with currentPage = {currentPage} and pageSize = {pageSize} using companyId {companyId} isDeleted = {isDeleted} and minUpdatedDate = {dateString}",
						CGHEnums.LogMessageType.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		#endregion Convenience Methods

		public async Task<bool> IsServiceOnlineAsync()
		{
			bool retVal = false;
			try
			{
				var client = GetClient(DefaultAuthenticationHeaderValue, DefaultRequestedVersion, DefaultConnectionIdentifier);
				string requestUrl = $"{ExecutionContext.BaseWebApiUrl}APIStatus/";
				HttpResponseMessage response = await client.GetAsync(requestUrl);
				string content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				// TODO: Logging - service may not be running.
				// Eat error for now.
				//throw;
			}

			return retVal;
		}
	}
}