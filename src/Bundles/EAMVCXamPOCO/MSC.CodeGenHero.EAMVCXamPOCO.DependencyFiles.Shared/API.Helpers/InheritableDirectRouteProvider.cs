using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace CodeGenHero.EAMVCXamPOCO.API.Helpers
{
	/// <summary>
	///
	/// </summary>
	/// <remarks>
	/// See: http://stackoverflow.com/questions/27463061/how-do-you-inherit-route-prefixes-at-the-controller-class-level-in-webapi/39625735#39625735
	/// See: http://stackoverflow.com/questions/19989023/net-webapi-attribute-routing-and-inheritance
	/// </remarks>
	public class InheritableDirectRouteProvider : DefaultDirectRouteProvider
	{
		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			// Inherit route attributes decorated on base class controller's actions
			return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetControllerRouteFactories(HttpControllerDescriptor controllerDescriptor)
		{
			// Inherit route attributes decorated on base class controller
			// GOTCHA: RoutePrefixAttribute doesn't show up here, even though we were expecting it to.
			//  Am keeping this here anyways, but am implementing an ugly fix by overriding GetRoutePrefix
			return controllerDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
		}

		protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
		{
			// Get the calling controller's route prefix
			var routePrefix = base.GetRoutePrefix(controllerDescriptor);

			// Iterate through each of the calling controller's base classes that inherit from HttpController
			var baseControllerType = controllerDescriptor.ControllerType.BaseType;
			while (typeof(IHttpController).IsAssignableFrom(baseControllerType))
			{
				// Get the base controller's route prefix, if it exists
				// GOTCHA: There are two RoutePrefixAttributes... System.Web.Http.RoutePrefixAttribute and System.Web.Mvc.RoutePrefixAttribute!
				//  Depending on your controller implementation, either one or the other might be used... checking against typeof(RoutePrefixAttribute)
				//  without identifying which one will sometimes succeed, sometimes fail.
				//  Since this implementation is generic, I'm handling both cases (note: commented out).  Preference would be to extend System.Web.Mvc and System.Web.Http
				var baseRoutePrefix = Attribute.GetCustomAttribute(baseControllerType, typeof(System.Web.Http.RoutePrefixAttribute)); // ?? Attribute.GetCustomAttribute(baseControllerType, typeof(System.Web.Mvc.RoutePrefixAttribute));
				if (baseRoutePrefix != null)
				{
					// A trailing slash is added by the system. Only add it if we're prefixing an existing string
					var trailingSlash = string.IsNullOrEmpty(routePrefix) ? "" : "/";
					// Prepend the base controller's prefix
					routePrefix = ((RoutePrefixAttribute)baseRoutePrefix).Prefix + trailingSlash + routePrefix;
				}

				// Traverse up the base hierarchy to check for all inherited prefixes
				baseControllerType = baseControllerType.BaseType;
			}

			return routePrefix;
		}
	}
}