using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CodeGenHero.Logging.Log4net
{
	public interface ILog4NetLoggingService
	{
		Enums.LogLevel CurrentLogLevel { get; set; }

		void Debug();
		Guid Debug(string message, int logMessageType, Exception ex, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
		Guid Error(string message, int logMessageType, Exception ex, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
		Guid Fatal(string message, int logMessageType, Exception ex, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
		Guid Info(string message, int logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
		Task TrimLogAsync();
		Guid Warn(string message, int logMessageType, Exception ex, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
	}
}