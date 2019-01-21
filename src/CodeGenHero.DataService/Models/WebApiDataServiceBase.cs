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

        public virtual int DefaultRequestedVersion { get; set; } = 1;

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

        //private static readonly Object _lock = new Object();
        private const int MAX_HTTPCLIENT_INSTANCES = 500;

        private static ConcurrentDictionary<string, HttpClient> _httpClients = new ConcurrentDictionary<string, HttpClient>();

        public int HttpClientInstanceCount
        {
            get
            {
                return _httpClients.Count;
            }
        }

        public virtual HttpClient GetClient(int requestedVersion = 1, string connectionIdentifier = null)
        {
            return GetClient(DefaultAuthenticationHeaderValue, requestedVersion, DefaultConnectionIdentifier);
        }

        // Cache up to this many http clients instances.
        /// <summary>
        ///
        /// </summary>
        /// <param name="authorization"></param>
        /// <param name="requestedVersion"></param>
        /// <param name="connectionIdentifier"></param>
        /// <remarks>Converted to use a static implementation because HttpClient, is actually a shared object. Under the covers it is reentrant and thread safe.</remarks>
        /// <seealso cref="https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/"/>
        /// <returns></returns>
        public virtual HttpClient GetClient(AuthenticationHeaderValue authorization, int requestedVersion, string connectionIdentifier)
        {
            string key = $"{requestedVersion}:{connectionIdentifier}:{authorization}";
            HttpClient retVal = null;

            //lock (_lock) // No need to lock the collection because it is thread-safe.
            //{
            if (_httpClients.Count > MAX_HTTPCLIENT_INSTANCES)
            {
                HttpClient itemToRemove = null;
                int itemsRemoved = 0;
                foreach (var keyToRemove in _httpClients.Keys)
                {
                    if (_httpClients.TryRemove(keyToRemove, out itemToRemove))
                    {
                        itemToRemove.Dispose();  // Release any resources associated with this HttpClient object.
                    }

                    itemsRemoved++;
                    if (itemsRemoved > MAX_HTTPCLIENT_INSTANCES / 10)
                    {   // We've cleand up 10%, exit.
                        break;
                    }
                }

                //_httpClients.Clear();
            }

            retVal = _httpClients.GetOrAdd(key, x =>
            {
                var newClient = GetNewHttpClient(authorization, requestedVersion, connectionIdentifier);
                return newClient;
            });
            //}

            return retVal;
        }

        private HttpClient GetNewHttpClient(AuthenticationHeaderValue authorization, int requestedVersion, string connectionIdentifier)
        {
            HttpClient retVal = new HttpClient
            {
                BaseAddress = new Uri(ExecutionContext.BaseWebApiUrl)
            };
            retVal.DefaultRequestHeaders.Accept.Clear();
            retVal.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (requestedVersion > 0)
            {
                // Using a custom request header
                retVal.DefaultRequestHeaders.Add("api-version", requestedVersion.ToString());

                // Using content negotiation
                //client.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/vnd.mmsapi.v"
                //        + requestedVersion + "+json"));
            }

            if (!string.IsNullOrEmpty(connectionIdentifier))
            {
                retVal.DefaultRequestHeaders.Add(cghConstants.CONNECTIONIDENTIFIER, connectionIdentifier);
            }

            if (authorization != null)
            {
                retVal.DefaultRequestHeaders.Authorization = authorization;
            }

            return retVal;
        }

        #endregion Client Code

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