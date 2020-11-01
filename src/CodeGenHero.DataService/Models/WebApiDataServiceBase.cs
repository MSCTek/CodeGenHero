using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using cghConstants = CodeGenHero.DataService.Constants;
using coreEnums = CodeGenHero.Core.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CodeGenHero.DataService
{
    public abstract class WebApiDataServiceBase : IWebApiDataServiceBase
    {
        #region Constructor and Context

        public WebApiDataServiceBase(ILogger log, HttpClient httpclient, string isServiceOnlineRelativeUrl = "APIStatus/")
        {
            Log = log;
            HttpClient = httpclient;
            IsServiceOnlineRelativeUrl = isServiceOnlineRelativeUrl;
        }

        public virtual HttpClient HttpClient { get; set; }
        public virtual string IsServiceOnlineRelativeUrl { get; set; }
        public virtual ILogger Log { get; set; }

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

                    Log.LogWarning(eventId: (int)coreEnums.EventId.Warn_WebApiClient,
                        exception: response?.Exception,
                        message: "Failure during WebApiClient to Web API GetAllPage operation with {ReturnTypeName}. ReasonPhrase: {ReasonPhrase}  HttpResponseStatusCode: {HttpResponseStatusCode} using CurrentPageDataRequest: {CurrentPageDataRequest}",
                        retVal?.GetType().Name,
                        response?.ReasonPhrase,
                        (int)response.StatusCode,
                        serializedCurrentPageDataRequest);

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
            HttpResponseMessage response = null;

            try
            {
                response = await this.HttpClient.GetAsync(IsServiceOnlineRelativeUrl);
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                Log.LogError(eventId: (int)coreEnums.EventId.Exception_WebApiClient,
                    exception: ex,
                    message: "Failure during WebApiClient to Web API {HttpVerb} operation with {ReturnTypeName}. HttpResponseStatusCode: {HttpResponseStatusCode} from RequestUri {RequestUri}",
                    Enum.GetName(typeof(Enums.HttpVerb), Enums.HttpVerb.Get),
                    retVal.GetType().Name,
                    response == null ? 0 : (int)response.StatusCode,
                    IsServiceOnlineRelativeUrl);
            }

            return retVal;
        }
    }
}