using CodeGenHero.EAMVCXamPOCO;
using CodeGenHero.EAMVCXamPOCO.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MSC.BingoBuzz.Xam.Services
{
    public class LoggingService : ILoggingService
    {
        public LoggingService()
        {
            CurrentLogLevel = Enums.LogLevel.All;
        }

        //TODO: rework the logging service to accept a dict of additional data and remove the returned guid.
        public Enums.LogLevel CurrentLogLevel { get; set; }

        public Guid Debug(string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            if (CurrentLogLevel >= Enums.LogLevel.Debug)
            {
                WriteLog(Enums.LogLevel.Debug, message, logMessageType, ex, userName, clientIPAddress, methodName, sourceFile, lineNumber, executionTimeInMilliseconds, httpResponseStatusCode, url);
            }
            return Guid.Empty;
        }

        public Guid Error(string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = default(decimal?), int? httpResponseStatusCode = default(int?), string url = null)
        {
            if (CurrentLogLevel >= Enums.LogLevel.Error)
            {
                WriteLog(Enums.LogLevel.Error, message, logMessageType, ex, userName, clientIPAddress, methodName, sourceFile, lineNumber, executionTimeInMilliseconds, httpResponseStatusCode, url);
            }
            return Guid.Empty;
        }

        public Guid Fatal(string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = default(decimal?), int? httpResponseStatusCode = default(int?), string url = null)
        {
            if (CurrentLogLevel >= Enums.LogLevel.Fatal)
            {
                WriteLog(Enums.LogLevel.Fatal, message, logMessageType, ex, userName, clientIPAddress, methodName, sourceFile, lineNumber, executionTimeInMilliseconds, httpResponseStatusCode, url);
            }
            return Guid.Empty;
        }

        public Guid Info(string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = default(decimal?), int? httpResponseStatusCode = default(int?), string url = null)
        {
            if (CurrentLogLevel >= Enums.LogLevel.Info)
            {
                WriteLog(Enums.LogLevel.Info, message, logMessageType, ex, userName, clientIPAddress, methodName, sourceFile, lineNumber, executionTimeInMilliseconds, httpResponseStatusCode, url);
            }
            return Guid.Empty;
        }

        public void SendAnalytics(string eventName)
        {
            Analytics.TrackEvent(eventName);
        }

        public Guid Warn(string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null, [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0, decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            if (CurrentLogLevel >= Enums.LogLevel.Warn)
            {
                WriteLog(Enums.LogLevel.Warn, message, logMessageType, ex, userName, clientIPAddress, methodName, sourceFile, lineNumber, executionTimeInMilliseconds, httpResponseStatusCode, url);
            }
            return Guid.Empty;
        }

        private void WriteLog(Enums.LogLevel logLevel, string message, Enums.LogMessageType logMessageType, Exception ex = null, string userName = null,
           string clientIPAddress = null, string methodName = null, string sourceFile = null, int lineNumber = 0, decimal? executionTimeInMilliseconds = default(decimal?),
           int? httpResponseStatusCode = default(int?), string url = null)
        {
            try
            {
                string abbrSourceName = string.Empty;
                int last = sourceFile.LastIndexOf("\\");
                if (last > 0)
                {
                    abbrSourceName = sourceFile.Substring(last, sourceFile.Length - last);
                }
                if (logLevel <= Enums.LogLevel.Error && ex != null)
                {
                    var dict = new Dictionary<string, string>
                    {
                       { "ex_message", ex.Message },
                       { "ex_stacktrace", ex.StackTrace},
                       { "method", $"{abbrSourceName}: {methodName}: {lineNumber}"}
                    };

                    Analytics.TrackEvent($"{CurrentLogLevel}: {message}", dict);
                }
                else
                {
                    Analytics.TrackEvent($"{CurrentLogLevel}: {message}");
                }

                if (ex != null)
                {
                    System.Diagnostics.Debug.WriteLine("******************************************************************");
                    System.Diagnostics.Debug.WriteLine($"ex.Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"ex.StackTrace: {ex.StackTrace}");
                    System.Diagnostics.Debug.WriteLine($"ex.InnerException.Message: {ex.InnerException?.Message}");
                    System.Diagnostics.Debug.WriteLine($"ex.InnerException.StackTrace: {ex.InnerException?.StackTrace}");
                    System.Diagnostics.Debug.WriteLine("******************************************************************");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"{DateTime.Now} {CurrentLogLevel}: {message}");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"FAILURE TO LOG! Serious application exception which is also blocking analytics functionality! Error: {e.Message} StackTrace: {e.StackTrace}");
            }
        }
    }
}