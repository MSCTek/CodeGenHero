// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;

namespace CodeGenHero.Core
{
	public static class Enums
	{
		public enum EventId
		{
			Unknown = 100,
			Exception_Application = 101,
			Exception_Database = 102,
			Exception_General = 103,
			Exception_Unhandled = 104,
			Exception_WebApi = 105,
			Exception_WebApiClient = 108,
			Exception_Synchronization = 110,
			Exception_Authenticate = 111,
			Warn_WebApi = 205,
			Warn_Mobile = 206,
			Warn_Web = 207,
			Warn_WebApiClient = 208,
			Warn_Synchronization = 210,
			Info_General = 300,
			Info_Diagnostics = 301,
			Info_Synchronization = 310,
			Unauthorized = 401,
			Authentication_Info = 500,
			Authentication_Fail = 501,
			Authentication_Success = 502,
			TemplateGenerationError = 1001,
			TemplateInitializeError = 1002,
			TemplateMetadataError = 1003,
			TemplateSettingError = 1004,
			FileTargetConflictError = 1005,
			FileTemplateOutputError = 1006,
			FileNotFound = 4004,
			DbContextLoad = 5001,
		}

		public enum LogLevel
		{
			Trace = 0,
			Debug = 1,
			Information = 2,
			Warning = 3,
			Error = 4,
			Critical = 5,
			None = 6
		}

		[Flags]
		public enum MetadataSourceType
		{
			SQLServer = 1,
			EFCore = 2,
			ReversePOCO = 4,
			OpenAPI = 8
		}

		//[Flags]
		//public enum TemplateStatus
		//{
		//	Idle = 1,
		//	Running = 2,
		//	Stopping = 4
		//}
	}
}