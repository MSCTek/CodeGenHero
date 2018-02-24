using CodeGenHero.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
	public partial class SerializationHelper : ISerializationHelper
	{
		public const string MEDIATYPEHEADERVALUE_BSON = "application/bson";
		private static readonly Lazy<SerializationHelper> _lazyInstance = new Lazy<SerializationHelper>(() => new SerializationHelper());

		private SerializationHelper()
		{
		}

		public static SerializationHelper Instance { get { return _lazyInstance.Value; } }

		public virtual async Task<HttpCallResult<T>> MakeWebApiBSONCall<T>(
			Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class
		{
			T retValData = default(T);
			HttpCallResult<T> retVal = new HttpCallResult<T>();

			try
			{
				HttpResponseMessage response = null;
				if (httpVerb == Enums.HttpVerb.Post)
				{
					response = await PostBsonAsync<T>(client, requestUri, item);
				}
				else
				{
					throw new NotImplementedException();
				}

				if (response.IsSuccessStatusCode)
				{
					using (BsonReader reader = new BsonReader(await response.Content.ReadAsStreamAsync()))
					{
						JsonSerializer serializer = new JsonSerializer();
						retValData = serializer.Deserialize<T>(reader);
					}
				}
				else
				{
					log.Warn($"Failure during WebApiClient to Web API {Enum.GetName(typeof(Enums.HttpVerb), httpVerb)} operation with {retValData?.GetType().Name}.", LogMessageType.Instance.Warn_WebApiClient, httpResponseStatusCode: (int)response.StatusCode, url: requestUri);
				}

				retVal = new HttpCallResult<T>(
					retValData, requestUri,
					response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
			}
			catch (Exception ex)
			{
				retVal.Exception = ex;
			}

			return retVal;
		}

		public virtual async Task<HttpCallResult<T>> MakeWebApiFromBodyCall<T>(
			Enums.HttpVerb httpVerb, ILoggingService log, HttpClient client, string requestUri, T item) where T : class
		{
			T retValData = default(T);
			HttpCallResult<T> retVal = new HttpCallResult<T>();

			try
			{
				string serializedItem = JsonConvert.SerializeObject(item);
				var inputMessage = new System.Net.Http.HttpRequestMessage
				{
					Content = new System.Net.Http.StringContent(serializedItem, System.Text.Encoding.UTF8, "application/json")
				};

				inputMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

				System.Net.Http.HttpResponseMessage response = null;
				if (httpVerb == Enums.HttpVerb.Post)
				{
					response = client.PostAsync(requestUri, inputMessage.Content).Result;
				}
				else if (httpVerb == Enums.HttpVerb.Put)
				{
					response = client.PutAsync(requestUri, inputMessage.Content).Result;
				}
				else
				{
					throw new NotImplementedException();
				}

				if (response.IsSuccessStatusCode)
				{
					var responseContent = response.Content.ReadAsStringAsync();
					retValData = JsonConvert.DeserializeObject<T>(responseContent.Result);
				}
				else
				{
					log.Warn($"Failure during WebApiClient to Web API {Enum.GetName(typeof(Enums.HttpVerb), httpVerb)} operation with {retValData?.GetType().Name}.", LogMessageType.Instance.Warn_WebApiClient, httpResponseStatusCode: (int)response.StatusCode, url: requestUri);
				}

				retVal = new HttpCallResult<T>(
					retValData, requestUri,
					response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
			}
			catch (Exception ex)
			{
				retVal.Exception = ex;
			}

			return retVal;
		}

		public virtual async Task<HttpCallResult<T>> SerializeCallResultsDelete<T>(
			ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class
		{
			var retVal = new HttpCallResult<T>();
			retVal.IsSuccessStatusCode = false;

			HttpResponseMessage response = null;
			string requestUri = webApiRequestUrl; ;

			try
			{
				response = await client.DeleteAsync(requestUri);
				// A successful response is essentially "No Content" so there is nothing to deserialize here.
				if (!response.IsSuccessStatusCode)
				{
					log.Warn(message: $"HttpClient delete call was not successful - reason: {response.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Warn_WebApiClient,
						httpResponseStatusCode: (int)response.StatusCode, url: requestUri);
				}

				retVal = new HttpCallResult<T>(
					null, requestUri,
					response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
			}
			catch (Exception ex)
			{
				log.Error(message: $"HttpClient delete call resulted in error - status code: {response?.StatusCode} reason: {response?.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Exception_WebApiClient, ex: ex,
					httpResponseStatusCode: (int)response?.StatusCode, url: requestUri);
			}

			return retVal;
		}

		public virtual async Task<HttpCallResult<T>> SerializeCallResultsGet<T>(
			ILoggingService log, HttpClient client, string webApiRequestUrl) where T : class
		{
			var retVal = new HttpCallResult<T>();
			T retValData = default(T);
			retVal.IsSuccessStatusCode = false;

			HttpResponseMessage response = null;
			string requestUri = webApiRequestUrl;

			try
			{
				response = await client.GetAsync(requestUri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					retValData = JsonConvert.DeserializeObject<T>(content); // new GuidBoolJsonConverter());
				}
				else
				{
					log.Warn(message: $"HttpClient get call was not successful - reason: {response.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Warn_WebApiClient,
						httpResponseStatusCode: (int)response.StatusCode, url: requestUri);
				}

				retVal = new HttpCallResult<T>(
					retValData, requestUri,
					response.IsSuccessStatusCode, response.StatusCode, response.ReasonPhrase);
			}
			catch (Exception ex)
			{
				int? statusCode = response == null ? null : (int?)response.StatusCode;
				log.Error(message: $"HttpClient get call resulted in error - status code: {response?.StatusCode} reason: {response?.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Exception_WebApiClient, ex: ex,
					httpResponseStatusCode: statusCode, url: requestUri);
			}

			return retVal;
		}

		public virtual async Task<PageData<T>> SerializeCallResultsGet<T>(
			ILoggingService log, HttpClient client, string webApiParameterlessPath,
			List<string> filter, int page, int pageSize) where T : class
		{
			var retVal = new PageData<T>();
			T retValData = default(T);
			retVal.IsSuccessStatusCode = false;

			HttpResponseMessage response = null;
			string webApiRequestUrl = null;

			try
			{
				webApiRequestUrl = AddFilterToPath(webApiParameterlessPath, filter);
				webApiRequestUrl = AddPagingToPath(webApiRequestUrl, page, pageSize);
				response = await client.GetAsync(webApiRequestUrl);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					retValData = JsonConvert.DeserializeObject<T>(content); // , new GuidBoolJsonConverter());
					retVal.Data = retValData;

					var headers = response.Headers;
					IEnumerable<string> values;
					headers.TryGetValues("X-Pagination", out values);

					var e = values.GetEnumerator();
					if (e.MoveNext())
					{
						var x = JsonConvert.DeserializeObject<PageData>(e.Current);
						retVal.CurrentPage = x.CurrentPage;
						retVal.NextPageLink = x.NextPageLink;
						retVal.PageSize = x.PageSize;
						retVal.PreviousPageLink = x.PreviousPageLink;
						retVal.TotalCount = x.TotalCount;
						retVal.TotalPages = x.TotalPages;
					}

					retVal.IsSuccessStatusCode = true;
				}
				else
				{
					log.Warn(message: $"HttpClient get call was not successful - status code: {response.StatusCode} reason: {response.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Warn_WebApiClient,
						httpResponseStatusCode: (int)response.StatusCode, url: webApiRequestUrl);
				}
			}
			catch (Exception ex)
			{
				int? statusCode = response == null ? null : (int?)response.StatusCode;
				log.Error(message: $"HttpClient get call resulted in error - status code: {response?.StatusCode} reason: {response?.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Exception_WebApiClient,
					ex: ex, httpResponseStatusCode: statusCode, url: webApiRequestUrl);
			}

			return retVal;
		}

		public virtual async Task<PageData<T>> SerializeCallResultsGet<T>(
			ILoggingService log, HttpClient client, string webApiParameterlessPath, List<string> fields,
			List<string> filter, int page, int pageSize) where T : class
		{
			var retVal = new PageData<T>();
			T retValData = default(T);
			retVal.IsSuccessStatusCode = false;

			HttpResponseMessage response = null;
			string webApiRequestUrl = null;

			try
			{
				webApiRequestUrl = AddFilterToPath(webApiParameterlessPath, filter);
				webApiRequestUrl = AddPagingToPath(webApiRequestUrl, page, pageSize);
				webApiRequestUrl = AddFieldsToPath(webApiRequestUrl, fields);
				response = await client.GetAsync(webApiRequestUrl);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					retValData = JsonConvert.DeserializeObject<T>(content); //, new GuidBoolJsonConverter());
					retVal.Data = retValData;

					var headers = response.Headers;
					IEnumerable<string> values;
					headers.TryGetValues("X-Pagination", out values);

					var e = values.GetEnumerator();
					if (e.MoveNext())
					{
						var x = JsonConvert.DeserializeObject<PageData>(e.Current);
						retVal.CurrentPage = x.CurrentPage;
						retVal.NextPageLink = x.NextPageLink;
						retVal.PageSize = x.PageSize;
						retVal.PreviousPageLink = x.PreviousPageLink;
						retVal.TotalCount = x.TotalCount;
						retVal.TotalPages = x.TotalPages;
					}

					retVal.IsSuccessStatusCode = true;
				}
				else
				{
					log.Warn(message: $"HttpClient get call was not successful - status code: {response.StatusCode} reason: {response.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Warn_WebApiClient,
						httpResponseStatusCode: (int)response.StatusCode, url: webApiRequestUrl);
				}
			}
			catch (Exception ex)
			{
				int? statusCode = response == null ? null : (int?)response.StatusCode;
				log.Error(message: $"HttpClient get call resulted in error - status code: {response?.StatusCode} reason: {response?.ReasonPhrase}.", logMessageType: LogMessageType.Instance.Exception_WebApiClient,
					ex: ex, httpResponseStatusCode: statusCode, url: webApiRequestUrl);
			}

			return retVal;
		}

		public virtual async Task<HttpCallResult<T>> SerializeCallResultsPost<T>(
			ILoggingService log, HttpClient client, string requestUri, T item) where T : class
		{
			return await MakeWebApiFromBodyCall<T>(Enums.HttpVerb.Post, log, client, requestUri, item);
		}

		public virtual async Task<HttpCallResult<T>> SerializeCallResultsPut<T>(
			ILoggingService log, HttpClient client, string requestUri, T item) where T : class
		{
			return await MakeWebApiFromBodyCall<T>(Enums.HttpVerb.Put, log, client, requestUri, item);
		}

		private string AddFieldsToPath(string requestURL, List<string> fields)
		{
			string retVal = requestURL;
			if (fields.Count == 0)
			{
				return retVal;
			}

			if (!retVal.Contains("?"))
			{
				retVal = $"{retVal}?";
			}
			else
			{
				retVal = $"{retVal}&";
			}

			return $"{retVal}fields={string.Join(",", fields)}";
		}

		private string AddFilterToPath(string requestURL, List<string> filter)
		{
			string retVal = requestURL;
			if (filter == null || filter.Count == 0)
				return retVal;

			if (!retVal.Contains("?"))
				retVal = $"{retVal}?";
			else
				retVal = $"{retVal}&";

			retVal = $"{retVal}filter=";

			for (int i = 0; i < filter.Count; i++)
			{
				if (i > 0)
					retVal = $"{retVal},";

				retVal = $"{retVal}{filter[i]}";
			}

			return retVal;
		}

		private string AddPagingToPath(
			string requestURL, int page, int pageSize)
		{
			string retVal = requestURL;

			if (!retVal.Contains("?"))
				retVal = $"{retVal}?";
			else
				retVal = $"{retVal}&";

			retVal = $"{retVal}page={page}&pageSize={pageSize}";

			return retVal;
		}

		private async Task<HttpResponseMessage> PostBsonAsync<T>(
			HttpClient client, string url, T data)
		{
			//Specifiy 'Accept' header As BSON: to ask server to return data as BSON format
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue(MEDIATYPEHEADERVALUE_BSON));

			//Specify 'Content-Type' header: to tell server which format of the data will be posted
			//Post data will be as Bson format
			var bsonData = HttpExtensions.SerializeBson<T>(data);
			var byteArrayContent = new ByteArrayContent(bsonData);
			byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(MEDIATYPEHEADERVALUE_BSON);

			var response = await client.PostAsync(url, byteArrayContent);
			return response;
		}
	}
}