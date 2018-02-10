using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CodeGenHero.EAMVCXamPOCO.Interface;
using appEnums = CodeGenHero.EAMVCXamPOCO.Enums;

namespace CodeGenHero.EAMVCXamPOCO.API.Helpers
{
	public class LogActionAttribute : ActionFilterAttribute
	{
		private static string STARTKEY = "RequestStart";

		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			var request = actionExecutedContext.Request;
			if (request != null && request.Properties.ContainsKey(STARTKEY))
			{
				DateTime startTime = (DateTime)request.Properties[STARTKEY];
				TimeSpan executionTime = DateTime.UtcNow - startTime;
				Decimal? executionTimeInMilliseconds = (Decimal?)executionTime.TotalMilliseconds;
				int responseStatusCode = (int)(actionExecutedContext.Response.StatusCode);

				var loggingServiceController = actionExecutedContext.ActionContext?.ControllerContext?.Controller
					as ILoggingService;
				if (loggingServiceController != null)
				{
					loggingServiceController.Info(message: $"WebApi invoked.", logMessageType: appEnums.LogMessageType.WebApi_PathAndQuery,
					clientIPAddress: GetClientIpAddress(request),
					executionTimeInMilliseconds: executionTimeInMilliseconds, httpResponseStatusCode: responseStatusCode,
					url: GetUrl(request));
				}

				//var baseAPIControllerAuthorized = actionExecutedContext.ActionContext?.ControllerContext?.Controller
				//    as MmsInstanceBaseApiControllerAuthorized;
				//if (baseAPIControllerAuthorized != null)
				//{
				//    baseAPIControllerAuthorized.Info(message: $"WebApi invoked.", logMessageType: LogMessageType.WebApi_PathAndQuery,
				//    clientIPAddress: GetClientIpAddress(request),
				//    executionTimeInMilliseconds: executionTimeInMilliseconds, httpResponseStatusCode: responseStatusCode,
				//    url: GetUrl(request));
				//}
				//else
				//{
				//    var baseAPIController = actionExecutedContext.ActionContext?.ControllerContext?.Controller
				//        as MmsInstanceBaseAPIController;
				//    if (baseAPIController != null)
				//    {
				//        baseAPIController.Info(message: $"WebApi invoked.", logMessageType: LogMessageType.WebApi_PathAndQuery,
				//        clientIPAddress: GetClientIpAddress(request),
				//        executionTimeInMilliseconds: executionTimeInMilliseconds, httpResponseStatusCode: responseStatusCode,
				//        url: GetUrl(request));
				//    }
				//}
			}

			//var objectContent = actionExecutedContext.Response.Content as ObjectContent;
			//if (objectContent != null)
			//{
			//    var type = objectContent.ObjectType; //type of the returned object
			//    var value = objectContent.Value; //holding the returned value
			//}

			//Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			// pre-processing
			var request = actionContext.Request;
			if (request != null)
			{
				request.Properties[STARTKEY] = DateTime.UtcNow;
			}
		}

		protected string GetClientIpAddress(HttpRequestMessage request)
		{
			return request?.GetClientIpAddress();
		}

		protected string GetUrl(HttpRequestMessage request)
		{
			return request?.RequestUri?.PathAndQuery;
		}
	}
}