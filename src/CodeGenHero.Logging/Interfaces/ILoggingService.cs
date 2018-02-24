using System;
using System.Runtime.CompilerServices;

namespace CodeGenHero.Logging
{
	public interface ILoggingService
	{
		CodeGenHero.Logging.Enums.LogLevel CurrentLogLevel { get; set; }

		Guid Debug(string message, int logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Error(string message, int logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Fatal(string message, int logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Info(string message, int logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Warn(string message, int logMessageType, Exception ex = null,
					string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
	}
}