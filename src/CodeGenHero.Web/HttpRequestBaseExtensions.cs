using System.Web;

namespace CodeGenHero.Web
{
	public static class HttpRequestBaseExtensions
	{
		public static string GetClientIpAddress(this HttpRequestBase request)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (!string.IsNullOrEmpty(ipAddress))
			{
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0)
				{
					return addresses[0];
				}
			}

			return context.Request.ServerVariables["REMOTE_ADDR"];
		}

		public static string GetUrl(this HttpRequestBase request)
		{
			if (request == null)
				return null;

			return request.Url?.PathAndQuery;
		}
	}
}