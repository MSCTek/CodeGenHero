using System;

namespace CodeGenHero.DataService
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

		//public enum ApiFilterOperator
		//{
		//	CONTAINS,
		//	DOESNOTCONTAIN,
		//	ENDSWITH,
		//	ISEMPTY,
		//	ISEQUALTO,
		//	ISGREATERTHAN,
		//	ISGREATERTHANOREQUAL,
		//	ISLESSTHAN,
		//	ISLESSTHANOREQUAL,
		//	ISNOTEMPTY,
		//	ISNOTEQUALTO,
		//	ISNOTNULL,
		//	ISNULL,
		//	STARTSWITH
		//}
	}
}