using CodeGenHero.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
	public interface ISerializationHelper
	{
		Task<HttpCallResult<T>> MakeWebApiBSONCall<T>(Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<HttpCallResult<T>> MakeWebApiFromBodyCall<T>(Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<HttpCallResult<T>> SerializeCallResultsDelete<T>(ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class;

		Task<HttpCallResult<T>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class;

		Task<PageData<T>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiParameterlessPath, List<string> filter, int page, int pageSize) where T : class;

		Task<PageData<T>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiParameterlessPath, List<string> fields, List<string> filter, int page, int pageSize) where T : class;

		Task<HttpCallResult<T>> SerializeCallResultsPost<T>(ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<HttpCallResult<T>> SerializeCallResultsPut<T>(ILoggingService log, HttpClient client, string requestUri, T item) where T : class;
	}
}