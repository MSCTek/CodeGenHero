using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cghConstants = CodeGenHero.DataService.Constants;
using CodeGenHero.Logging;

namespace CodeGenHero.DataService
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

		public virtual AuthenticationHeaderValue DefaultAuthenticationHeaderValue { get; set; }

		public virtual string DefaultConnectionIdentifier
		{
			get
			{
				string retVal = this.ExecutionContext?.ConnectionIdentifier;
				return retVal;
			}
		}

		public virtual int DefaultRequestedVersion { get; set; }

		public virtual IWebApiExecutionContext ExecutionContext { get; set; }

		public virtual ILoggingService Log { get; set; }
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

		public virtual HttpClient GetClient(int requestedVersion = 1, string connectionIdentifier = null)
		{
			return GetClient(DefaultAuthenticationHeaderValue, requestedVersion, DefaultConnectionIdentifier);
		}

		public virtual HttpClient GetClient(AuthenticationHeaderValue authorization, int requestedVersion, string connectionIdentifier)
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
				client.DefaultRequestHeaders.Add(cghConstants.CONNECTIONIDENTIFIER, connectionIdentifier);
			}

			if (authorization != null)
			{
				client.DefaultRequestHeaders.Authorization = authorization;
			}

			return client;
		}

		#endregion Client Code

		#region Convenience Methods

		protected virtual List<string> BuildFilter(Dictionary<string, string> filters, DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			var retVal = new List<string>();

			if (filters != null)
			{
				foreach (KeyValuePair<string, string> pair in filters)
				{
					retVal.Add($"{pair.Key}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISEQUALTO}{cghConstants.API_FILTER_DELIMITER}{pair.Value}");
				}
			}

			if (minUpdatedDate.HasValue && !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISGREATERTHANOREQUAL}{cghConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected virtual List<string> BuildFilter(Guid? companyId, DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			return BuildFilter(companyId, null, minUpdatedDate, updatedDateFieldName);
		}

		protected virtual List<string> BuildFilter(Guid? companyId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISEQUALTO}{cghConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISGREATERTHANOREQUAL}{cghConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected virtual List<string> BuildFilter(bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISEQUALTO}{cghConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISGREATERTHANOREQUAL}{cghConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected virtual List<string> BuildFilter(short? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISEQUALTO}{cghConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISGREATERTHANOREQUAL}{cghConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected virtual List<string> BuildFilter(DateTime? minUpdatedDate, string updatedDateFieldName)
		{
			return BuildFilter(Guid.Empty, minUpdatedDate, updatedDateFieldName);
		}

		protected virtual List<string> BuildFilterForCompanyCatalog(Guid companyCatalogId, bool? isDeleted, DateTime? minUpdatedDate, string updatedDateFieldName)

		{
			var retVal = new List<string>();

			if (isDeleted.HasValue)
			{
				retVal.Add($"isDeleted{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISEQUALTO}{cghConstants.API_FILTER_DELIMITER}{isDeleted}");
			}

			if (minUpdatedDate.HasValue
				&& !string.IsNullOrEmpty(updatedDateFieldName))
			{   // Construct the URL to match - //updateddate~IsGreaterThanOrEqual~1%2F15%2F2020
				var encodedDateString = System.Net.WebUtility.UrlEncode(minUpdatedDate.Value.ToString());
				retVal.Add($"{updatedDateFieldName}{cghConstants.API_FILTER_DELIMITER}{cghConstants.OPERATOR_ISGREATERTHANOREQUAL}{cghConstants.API_FILTER_DELIMITER}{encodedDateString}");
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<int, int,
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

					LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<bool?, DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<short?, DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<Guid, bool?, DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<Guid?, bool?, DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(Func<int, Guid?, bool?, DateTime?, int, int,
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
						LogMessageType.Instance.Exception_WebApi);
				}

				currentPage++;
			}

			return retVal;
		}

		#endregion Convenience Methods

		public virtual async Task<bool> IsServiceOnlineAsync()
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