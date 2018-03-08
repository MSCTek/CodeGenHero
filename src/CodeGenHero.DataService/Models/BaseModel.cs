using CodeGenHero.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeGenHero.DataService
{
	public abstract class BaseModel<T>
	{
		private IDataService<T> _dataService;
		private ILoggingService _log;
		protected const int UNKNOWNLOGMESSAGETYPE = 100;

		public BaseModel(ILoggingService log, IDataService<T> dataService)
		{
			_log = log;
			_dataService = dataService;

			ErrorMessages = new List<string>();
		}

		public virtual IDataService<T> DataService
		{
			get
			{
				return _dataService;
			}
		}

		public virtual List<string> ErrorMessages { get; set; }

		public virtual ILoggingService Log
		{
			get
			{
				return _log;
			}
		}

		public virtual void HandleLazyLoadRequest(object sender, EventArgs e)
		{
			// Override this method in a custom class to load properties that are not contained in the core DTO.
		}

		public virtual void LogDebug(Exception ex, int logMessageType = UNKNOWNLOGMESSAGETYPE,
			[CallerMemberName] string methodName = "",
			[CallerFilePath] string sourceFile = "",
			[CallerLineNumber] int lineNumber = 0)
		{
			StringBuilder sb = new StringBuilder();

			while (ex != null)
			{
				sb.AppendLine($"{ex.Message} {ex.StackTrace}");
				ex = ex.InnerException;
			}

			LogDebug(message: sb.ToString(), logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogDebug(string message, int logMessageType = UNKNOWNLOGMESSAGETYPE,
		[CallerMemberName] string methodName = "",
		[CallerFilePath] string sourceFile = "",
		[CallerLineNumber] int lineNumber = 0)
		{
			_log.Debug(message: message, logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogError(Exception ex, int logMessageType = UNKNOWNLOGMESSAGETYPE,
			[CallerMemberName] string methodName = "",
			[CallerFilePath] string sourceFile = "",
			[CallerLineNumber] int lineNumber = 0)
		{
			StringBuilder sb = new StringBuilder();

			while (ex != null)
			{
				sb.AppendLine($"{ex.Message} {ex.StackTrace}");
				ex = ex.InnerException;
			}

			LogError(message: sb.ToString(), logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogError(string message, int logMessageType = UNKNOWNLOGMESSAGETYPE,
		[CallerMemberName] string methodName = "",
		[CallerFilePath] string sourceFile = "",
		[CallerLineNumber] int lineNumber = 0)
		{
			_log.Error(message: message, logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogFatal(Exception ex, int logMessageType = UNKNOWNLOGMESSAGETYPE,
			[CallerMemberName] string methodName = "",
			[CallerFilePath] string sourceFile = "",
			[CallerLineNumber] int lineNumber = 0)
		{
			StringBuilder sb = new StringBuilder();

			while (ex != null)
			{
				sb.AppendLine($"{ex.Message} {ex.StackTrace}");
				ex = ex.InnerException;
			}

			LogFatal(message: sb.ToString(), logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogFatal(string message, int logMessageType = UNKNOWNLOGMESSAGETYPE,
		[CallerMemberName] string methodName = "",
		[CallerFilePath] string sourceFile = "",
		[CallerLineNumber] int lineNumber = 0)
		{
			_log.Fatal(message: message, logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogInfo(Exception ex, int logMessageType = UNKNOWNLOGMESSAGETYPE,
			[CallerMemberName] string methodName = "",
			[CallerFilePath] string sourceFile = "",
			[CallerLineNumber] int lineNumber = 0)
		{
			StringBuilder sb = new StringBuilder();

			while (ex != null)
			{
				sb.AppendLine($"{ex.Message} {ex.StackTrace}");
				ex = ex.InnerException;
			}

			LogInfo(message: sb.ToString(), logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}

		public virtual void LogInfo(string message, int logMessageType = UNKNOWNLOGMESSAGETYPE,
		[CallerMemberName] string methodName = "",
		[CallerFilePath] string sourceFile = "",
		[CallerLineNumber] int lineNumber = 0)
		{
			_log.Info(message: message, logMessageType: logMessageType,
				methodName: methodName,
				sourceFile: sourceFile,
				lineNumber: lineNumber);
		}
	}
}