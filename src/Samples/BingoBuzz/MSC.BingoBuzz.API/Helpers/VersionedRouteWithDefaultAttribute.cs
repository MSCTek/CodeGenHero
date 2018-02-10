using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace CodeGenHero.EAMVCXamPOCO.API.Helpers
{
    /// <summary>
    /// Provides an attribute route that's restricted to a specific version of the api.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    internal class VersionedRouteWithDefaultAttribute : RouteFactoryAttribute
    {
        public VersionedRouteWithDefaultAttribute(string template, int allowedVersion, int defaultVersion)
            : base(template)
        {
            AllowedVersion = allowedVersion;
            DefaultVersion = defaultVersion;
        }

        public int AllowedVersion
        {
            get;
            private set;
        }

        public int? DefaultVersion
        {
            get;
            private set;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint(AllowedVersion, DefaultVersion));
                return constraints;
            }
        }
    }
}