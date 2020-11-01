//using Microsoft.Extensions.Logging;
//using System;
//using System.Net.Http;
//using System.Web.Http.Controllers;
//using System.Web.Http.Filters;

//namespace CodeGenHero.WebApi
//{
//    public class LogActionAttribute : ActionFilterAttribute
//    {
//        private static string STARTKEY = "RequestStart";

//        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
//        {
//            var request = actionExecutedContext.Request;
//            if (request != null && request.Properties.ContainsKey(STARTKEY))
//            {
//                DateTime startTime = (DateTime)request.Properties[STARTKEY];
//                TimeSpan executionTime = DateTime.UtcNow - startTime;
//                Decimal? executionTimeInMilliseconds = (Decimal?)executionTime.TotalMilliseconds;
//                int responseStatusCode = (int)(actionExecutedContext.Response.StatusCode);

//                var loggingServiceController = actionExecutedContext.ActionContext?.ControllerContext?.Controller
//                    as ILoggingService;
//                if (loggingServiceController != null)
//                {
//                    loggingServiceController.Info(message: $"WebApi invoked.", logMessageType: LogMessageType.Instance.WebApi_PathAndQuery,
//                    clientIPAddress: GetClientIpAddress(request),
//                    executionTimeInMilliseconds: executionTimeInMilliseconds, httpResponseStatusCode: responseStatusCode,
//                    url: GetUrl(request));
//                }
//            }
//        }

//        public override void OnActionExecuting(HttpActionContext actionContext)
//        {
//            // pre-processing
//            var request = actionContext.Request;
//            if (request != null)
//            {
//                request.Properties[STARTKEY] = DateTime.UtcNow;
//            }
//        }

//        protected string GetClientIpAddress(HttpRequestMessage request)
//        {
//            return request?.GetClientIpAddress();
//        }

//        protected string GetUrl(HttpRequestMessage request)
//        {
//            return request?.RequestUri?.PathAndQuery;
//        }
//    }
//}