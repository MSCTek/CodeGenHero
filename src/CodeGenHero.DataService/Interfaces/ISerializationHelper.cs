using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
    public interface ISerializationHelper
    {
        Task<IHttpCallResultCGHT<T>> MakeWebApiBSONCall<T>(Enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> MakeWebApiFromBodyCall<T>(Enums.HttpVerb httpVerb, ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsDelete<T>(ILogger log, HttpClient client, string webApiRequestUrl) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsGet<T>(ILogger log, HttpClient client, string webApiRequestUrl) where T : class;

        Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILogger log, HttpClient client, string webApiParameterlessPath, IList<string> filter, int page, int pageSize) where T : class;

        Task<IHttpCallResultCGHT<IPageDataT<T>>> SerializeCallResultsGet<T>(ILogger log, HttpClient client, string webApiParameterlessPath, IList<string> fields, IList<string> filter, int page, int pageSize) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsPost<T>(ILogger log, HttpClient client, string requestUri, T item) where T : class;

        Task<IHttpCallResultCGHT<T>> SerializeCallResultsPut<T>(ILogger log, HttpClient client, string requestUri, T item) where T : class;
    }
}