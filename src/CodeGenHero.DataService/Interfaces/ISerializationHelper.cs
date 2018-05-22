using CodeGenHero.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
	public interface ISerializationHelper
	{
		Task<IHttpCallResultCGHT<T>> MakeWebApiBSONCall<T>(Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<IHttpCallResultCGHT<T>> MakeWebApiFromBodyCall<T>(Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<IHttpCallResultCGHT<T>> SerializeCallResultsDelete<T>(ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class;

		Task<IHttpCallResultCGHT<T>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class;

		Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiParameterlessPath, IList<string> filter, int page, int pageSize) where T : class;

		Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILoggingService log, HttpClient client, string webApiParameterlessPath, IList<string> fields, IList<string> filter, int page, int pageSize) where T : class;

		Task<IHttpCallResultCGHT<T>> SerializeCallResultsPost<T>(ILoggingService log, HttpClient client, string requestUri, T item) where T : class;

		Task<IHttpCallResultCGHT<T>> SerializeCallResultsPut<T>(ILoggingService log, HttpClient client, string requestUri, T item) where T : class;
	}
}