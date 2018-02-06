using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CodeGenHero.EAMVCXamPOCO.API.Helpers
{
	public static class HttpRequestMessageExtensions
	{
		private const string HttpContext = "MS_HttpContext";
		private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

		public static string GetClientIpAddress(this HttpRequestMessage request)
		{
			var owinContext = request.GetOwinContext();
			if (owinContext != null && owinContext.Request != null)
			{
				return owinContext.Request.RemoteIpAddress;
			}

			if (request.Properties.ContainsKey(HttpContext))
			{
				dynamic ctx = request.Properties[HttpContext];
				if (ctx != null)
				{
					return ctx.Request.UserHostAddress;
				}
			}

			if (request.Properties.ContainsKey(RemoteEndpointMessage))
			{
				dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
				if (remoteEndpoint != null)
				{
					return remoteEndpoint.Address;
				}
			}

			return null;
		}

		public static T GetFirstHeaderValueOrDefault<T>(
				this HttpRequestMessage request,
				string headerKey)
		{
			var toReturn = default(T);

			IEnumerable<string> headerValues;

			if (request.Headers.TryGetValues(headerKey, out headerValues))
			{
				var valueString = headerValues.FirstOrDefault();
				if (valueString != null)
				{
					return (T)Convert.ChangeType(valueString, typeof(T));
				}
			}

			return toReturn;
		}
	}
}