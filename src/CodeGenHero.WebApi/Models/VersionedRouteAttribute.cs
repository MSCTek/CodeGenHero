using System;
using System.Collections.Generic;
using System.Web.Http.Routing;

namespace CodeGenHero.WebApi
{
	/// <summary>
	/// Provides an attribute route that's restricted to a specific version of the api.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class VersionedRouteAttribute : RouteFactoryAttribute
	{
		public VersionedRouteAttribute(string template, int allowedVersion)
			: base(template)
		{
			AllowedVersion = allowedVersion;
		}

		public int AllowedVersion
		{
			get;
			private set;
		}

		public override IDictionary<string, object> Constraints
		{
			get
			{
				var constraints = new HttpRouteValueDictionary();
				constraints.Add("version", new VersionConstraint(AllowedVersion));
				return constraints;
			}
		}
	}
}