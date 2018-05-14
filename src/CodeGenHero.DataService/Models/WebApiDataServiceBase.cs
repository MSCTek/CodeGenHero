using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cghConstants = CodeGenHero.DataService.Constants;
using CodeGenHero.Logging;
using Newtonsoft.Json;

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
			HttpClient client = new HttpClient
			{
				BaseAddress = new Uri(ExecutionContext.BaseWebApiUrl)
			};
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

		protected virtual List<string> BuildFilter(List<IFilterCriterion> filterCriteria)
		{
			var retVal = new List<string>();

			if (filterCriteria != null)
			{
				foreach (IFilterCriterion filterCriterion in filterCriteria)
				{
					retVal.Add($"{filterCriterion.FieldName}{cghConstants.API_FILTER_DELIMITER}{filterCriterion.FilterOperator}{cghConstants.API_FILTER_DELIMITER}{filterCriterion.Value}");
				}
			}

			return retVal;
		}

		protected virtual async Task<List<T>> GetAllPageDataResultsAsync<T>(IPageDataRequest pageDataRequest, Func<IPageDataRequest,
			Task<IHttpCallResultCGHT<IPageDataT<IList<T>>>>> getMethodToRun, bool throwExceptionOnFailureStatusCode = false)
		{
			List<T> retVal = new List<T>();
			IHttpCallResultCGHT<IPageDataT<IList<T>>> response = null;
			IPageDataRequest currentPageDataRequest = new PageDataRequest(pageDataRequest.FilterCriteria, pageDataRequest.Sort, pageDataRequest.Page, pageDataRequest.PageSize);

			while (response == null
				|| (response.IsSuccessStatusCode == true && currentPageDataRequest.Page <= response.Data.TotalPages))
			{
				response = await getMethodToRun(currentPageDataRequest);
				if (response.IsSuccessStatusCode && response.Data != null)
				{
					retVal.AddRange(response.Data.Data);
				}
				else
				{
					string serializedCurrentPageDataRequest = JsonConvert.SerializeObject(currentPageDataRequest);
					string msg = $"{nameof(GetAllPageDataResultsAsync)} call resulted in error - status code: {response?.StatusCode}; reason: {response?.ReasonPhrase}. CurrentPageDataRequest: {serializedCurrentPageDataRequest}";

					Log.Error(message: msg, logMessageType: LogMessageType.Instance.Exception_WebApiClient, ex: response?.Exception,
						httpResponseStatusCode: (int)response?.StatusCode, url: null);

					if (throwExceptionOnFailureStatusCode == true)
					{
						throw new ApplicationException(msg, response?.Exception);
					}
				}

				currentPageDataRequest.Page += 1;
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