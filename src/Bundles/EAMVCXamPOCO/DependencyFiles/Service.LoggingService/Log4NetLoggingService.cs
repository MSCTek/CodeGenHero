using CodeGenHero.EAMVCXamPOCO.Interface;
using log4net;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static CodeGenHero.EAMVCXamPOCO.Enums;

namespace CodeGenHero.EAMVCXamPOCO.Service
{
	public class Log4NetLoggingService : ILoggingService //, log4net.ILog
    {
        //private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ILog _log = null; // Let's lazy-load this to save processing in case it gets dependency injected and not used.

        public Log4NetLoggingService()
        {
            // The following is not necessary - we use assembly declarations in AssemblyInfo.cs instead
            //[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
            //XmlElement log4NetSection = (XmlElement)ConfigurationManager.GetSection("log4net");
            //log4net.Config.XmlConfigurator.Configure(log4NetSection);
        }

        public LogLevel CurrentLogLevel
        {
            get
            {   // Based on log4net.Core.Level
                if (CurrentLogger.IsDebugEnabled)
                    return LogLevel.Debug;
                else if (CurrentLogger.IsInfoEnabled)
                    return LogLevel.Info;
                else if (CurrentLogger.IsWarnEnabled)
                    return LogLevel.Warn;
                else if (CurrentLogger.IsErrorEnabled)
                    return LogLevel.Error;
                else if (CurrentLogger.IsFatalEnabled)
                    return LogLevel.Fatal;
                else
                    return LogLevel.Off;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        protected ILog CurrentLogger
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                }

                return _log;
            }
        }

        public void Debug()
        {
            throw new NotImplementedException();
        }

        public Guid Debug(string message, LogMessageType logMessageType, Exception ex, string userName = null, string clientIPAddress = null,
        [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            var logGuid = System.Guid.NewGuid();
            WriteDebugDiagnostics(message, logMessageType, ex, userName, clientIPAddress,
                methodName, sourceFile, lineNumber, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);
            ILog log = SetCustomContextProperties(logMessageType, userName,
                clientIPAddress, methodName, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);

            if (log.IsDebugEnabled)
            {
                if (ex != null)
                {
                    log.Debug(message, ex);
                }
                else
                {
                    log.Debug(message);
                }
            }

            return logGuid;
        }

        public Guid Error(string message, LogMessageType logMessageType, Exception ex, string userName = null, string clientIPAddress = null,
            [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            var logGuid = System.Guid.NewGuid();
            WriteDebugDiagnostics(message, logMessageType, ex, userName, clientIPAddress,
                methodName, sourceFile, lineNumber, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);
            ILog log = SetCustomContextProperties(logMessageType, userName,
                clientIPAddress, methodName, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);

            if (log.IsErrorEnabled)
            {
                if (ex != null)
                {
                    log.Error(message, ex);
                }
                else
                {
                    log.Error(message);
                }
            }

            return logGuid;
        }

        public Guid Fatal(string message, LogMessageType logMessageType, Exception ex, string userName = null, string clientIPAddress = null,
            [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            var logGuid = System.Guid.NewGuid();
            WriteDebugDiagnostics(message, logMessageType, ex, userName, clientIPAddress,
                methodName, sourceFile, lineNumber, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);
            ILog log = SetCustomContextProperties(logMessageType, userName,
                clientIPAddress, methodName, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);

            if (log.IsFatalEnabled)
            {
                if (ex != null)
                {
                    log.Fatal(message, ex);
                }
                else
                {
                    log.Fatal(message);
                }
            }

            return logGuid;
        }

        public Guid Info(string message, LogMessageType logMessageType, Exception ex = null, string userName = null, string clientIPAddress = null,
            [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            var logGuid = System.Guid.NewGuid();
            WriteDebugDiagnostics(message, logMessageType, ex, userName, clientIPAddress,
                methodName, sourceFile, lineNumber, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);
            ILog log = SetCustomContextProperties(logMessageType, userName,
                clientIPAddress, methodName, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);

            if (log.IsInfoEnabled)
            {
                if (ex != null)
                {
                    log.Info(message, ex);
                }
                else
                {
                    log.Info(message);
                }
            }

            return logGuid;
        }

        public Task TrimLogAsync()
        {
            throw new NotImplementedException();
        }

        public Guid Warn(string message, LogMessageType logMessageType, Exception ex, string userName = null, string clientIPAddress = null,
            [CallerMemberName] string methodName = null, [CallerFilePath] string sourceFile = null, [CallerLineNumber] int lineNumber = 0,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            var logGuid = System.Guid.NewGuid();
            WriteDebugDiagnostics(message, logMessageType, ex, userName, clientIPAddress,
                methodName, sourceFile, lineNumber, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);
            ILog log = SetCustomContextProperties(logMessageType, userName,
                clientIPAddress, methodName, logGuid, executionTimeInMilliseconds, httpResponseStatusCode, url);

            if (log.IsWarnEnabled)
            {
                if (ex != null)
                {
                    log.Warn(message, ex);
                }
                else
                {
                    log.Warn(message);
                }
            }

            return logGuid;
        }

        private string EscapeForJSON(string valueToEscape)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(valueToEscape))
            {
                retVal = valueToEscape.Replace(@"\", @"\\").Replace("\"", "\"\""); // Escape any solidus and/or quotation marks.
            }

            return retVal;
        }

        private string GetExceptionString(Exception ex)
        {
            string retVal = string.Empty;

            if (ex != null)
            {
                retVal = string.Format("Exception type = {0}, message = {1}.", ex.GetType().ToString(), ex.Message);
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    retVal += string.Format("  Inner type = {0}, message = {1}.", ex.GetType().ToString(), ex.Message);
                }
            }

            return EscapeForJSON(retVal);
        }

        private ILog SetCustomContextProperties(LogMessageType logMessageType,
            string userName, string clientIPAddress,
            string methodName, Guid logGuid,
            Decimal? executionTimeInMilliseconds, int? httpResponseStatusCode, string url)
        {
            ILog retVal = this.CurrentLogger;

            log4net.LogicalThreadContext.Properties["LogMessageTypeID"] = (int)logMessageType;
            log4net.LogicalThreadContext.Properties["MethodName"] = methodName;
            log4net.LogicalThreadContext.Properties["UserName"] = userName ?? string.Empty;
            log4net.LogicalThreadContext.Properties["ClientIPAddress"] = clientIPAddress ?? string.Empty;
            log4net.LogicalThreadContext.Properties["LogGuid"] = logGuid;
            log4net.LogicalThreadContext.Properties["ExecutionTimeInMilliseconds"] = executionTimeInMilliseconds;
            log4net.LogicalThreadContext.Properties["HttpResponseStatusCode"] = httpResponseStatusCode;
            log4net.LogicalThreadContext.Properties["Url"] = url;

            return retVal;
        }

        [Conditional("DEBUG")]
        private void WriteDebugDiagnostics(string message, LogMessageType logMessageType, Exception ex, string userName, string clientIPAddress,
            string methodName, string sourceFile, int lineNumber, Guid logGuid,
            Decimal? executionTimeInMilliseconds = null, int? httpResponseStatusCode = null, string url = null)
        {
            //Desired string format: {"logMessageTypeID":0,"userName":"","message":"Successful login for admin.","exception":"","methodName":"Login","sourceFile":"c:\\TFS","lineNumber": 48, "logGuid" : "A5B5A056-E942-415A-84F9-8ADD64AB126A"}
            var msg = $@"{{""logGuid"": ""{logGuid}"", ""logMessageType"": ""{Enum.GetName(typeof(LogMessageType), logMessageType)}"",""userName"": ""{userName}"",""clientIPAddress"": ""{clientIPAddress}"",""message"": ""{message}"",""exception"": ""{GetExceptionString(ex)}"",""methodName"": ""{methodName}"",""sourceFile"": ""{EscapeForJSON(sourceFile)}"",""lineNumber"": {lineNumber}, ""executionTimeInMilliseconds"": ""{executionTimeInMilliseconds}"", ""httpResponseStatusCode"": ""{httpResponseStatusCode}"", ""url"": ""{url}""}}";
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}