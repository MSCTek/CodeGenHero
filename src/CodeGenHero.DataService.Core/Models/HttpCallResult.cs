using System;
using System.Net;

namespace CodeGenHero.DataService
{
	public class HttpCallResult<T> : HttpCallResult, IHttpCallResultCGHT<T>
	{
		public HttpCallResult() : base()
		{
		}

		public HttpCallResult(T data) : base()
		{
			Data = data;
		}

		public HttpCallResult(T data, string requestUri, bool isSuccessStatusCode, HttpStatusCode statusCode, string reasonPhrase, Exception exception = null)
			: base(requestUri, isSuccessStatusCode, statusCode, reasonPhrase, exception)
		{
			Data = data;
		}

		public T Data { get; set; }
	}

	public class HttpCallResult : IHttpCallResultCGH
	{
		public HttpCallResult()
		{
		}

		public HttpCallResult(string requestUri, bool isSuccessStatusCode, HttpStatusCode statusCode, string reasonPhrase, Exception exception = null)
		{
			RequestUri = requestUri;
			IsSuccessStatusCode = isSuccessStatusCode;
			StatusCode = statusCode;
			ReasonPhrase = reasonPhrase;
			this.Exception = exception;
		}

		public long? ContentLength { get; set; }
		public string ContentType { get; set; }
		public Exception Exception { get; set; }
		public string FileName { get; set; }
		public bool IsSuccessStatusCode { get; set; }
		public string MediaType { get; set; }
		public string ReasonPhrase { get; set; }
		public string RequestUri { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}