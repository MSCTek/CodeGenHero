using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cghConstants = CodeGenHero.DataService.Constants;
using CodeGenHero.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace CodeGenHero.DataService
{
	public abstract class WebApiDataServiceBase : IWebApiDataServiceBase
	{
		private HttpClient _httpclient;
		private string _isServiceOnlineRelativeUrl;

		#region Constructor and Context

		public WebApiDataServiceBase(ILoggingService log, HttpClient httpclient, string isServiceOnlineRelativeUrl = "APIStatus/")
		{
			Log = log;
			HttpClient = httpclient;
			IsServiceOnlineRelativeUrl = isServiceOnlineRelativeUrl;
		}

		public virtual HttpClient HttpClient { get; set; }
		public virtual string IsServiceOnlineRelativeUrl { get; set; }
		public virtual ILoggingService Log { get; set; }

		#endregion Constructor and Context

		#region Convenience Methods

		protected virtual List<string> BuildFilter(IList<IFilterCriterion> filterCriteria)
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
				HttpResponseMessage response = await HttpClient.GetAsync(IsServiceOnlineRelativeUrl);
				string content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					retVal = true;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, logMessageType: LogMessageType.Instance.Exception_WebApiClient, ex: ex);
			}

			return retVal;
		}
	}
}