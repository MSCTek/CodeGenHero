using System;

namespace CodeGenHero.EAMVCXamPOCO
{
	public static class Enums
	{
		public enum HttpVerb
		{
			Delete, //Requests that a specified URI be deleted.
			Get, //Retrieves the information or entity that is identified by the URI of the request.
			Head, //Retrieves the message headers for the information or entity that is identified by the URI of the request.
			Options, //Represents a request for information about the communication options available on the request/response chain identified by the Request-URI.
			Patch, //Requests that a set of changes described in the request entity be applied to the resource identified by the Request- URI.
			Post, //Posts a new entity as an addition to a URI.
			Put //Replaces an entity that is identified by a URI.
		}

		[Flags]
		public enum LogLevel
		{   // Based on log4net.Core.Level
			Off = 0,
			Fatal = 1,
			Error = 2,
			Warn = 4,
			Info = 8,
			Debug = 16,
			All = 31
		}

		public enum LogMessageType
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
			WebApi_PathAndQuery = 1001,
			ExcessiveImageSize = 1002,
		}

		[Flags]
		public enum RepositoryActionStatus
		{
			Ok = 1,
			Created = 2,
			Updated = 4,
			NotFound = 8,
			Deleted = 16,
			NothingModified = 32,
			Error = 64
		}

		[Flags]
		public enum WebApiAuthenticationHeaderValueType
		{
			BasicAuth = 1,
			BearerToken = 2,
		}
	}
}