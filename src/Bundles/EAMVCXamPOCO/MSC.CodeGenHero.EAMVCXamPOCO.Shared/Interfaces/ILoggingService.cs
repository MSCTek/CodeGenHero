using System;
using System.Runtime.CompilerServices;
using static CodeGenHero.EAMVCXamPOCO.Enums;

namespace CodeGenHero.EAMVCXamPOCO.Interface
{
	public interface ILoggingService
	{
		LogLevel CurrentLogLevel { get; set; }

		Guid Debug(string message, LogMessageType logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Error(string message, LogMessageType logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Fatal(string message, LogMessageType logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Info(string message, LogMessageType logMessageType, Exception ex = null,
			string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);

		Guid Warn(string message, LogMessageType logMessageType, Exception ex = null,
					string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null,
			[CallerFilePath] string sourceFile = null, [CallerLineNumber]  int lineNumber = 0,
			Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null);
	}
}