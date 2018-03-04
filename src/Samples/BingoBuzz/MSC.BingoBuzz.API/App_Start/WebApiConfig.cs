using CodeGenHero.WebApi;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MSC.BingoBuzz.API
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			//if (System.Diagnostics.Debugger.IsAttached)
			//	config.EnableSystemDiagnosticsTracing();

			#region Custom Controller Selector

			// Web API routes
			config.MapHttpAttributeRoutes(new InheritableDirectRouteProvider()); // Allow for inheritance of the RoutePrefix attribute (see: stackoverflow.com/questions/19989023/net-webapi-attribute-routing-and-inheritance)

			#endregion Custom Controller Selector

			// clear the supported mediatypes of the xml formatter
			config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
				new MediaTypeHeaderValue("application/json-patch+json"));

			config.Formatters.Add(new BsonMediaTypeFormatter());

			var json = config.Formatters.JsonFormatter;
			json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
			json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			LogActionAttribute laa = new LogActionAttribute();
			config.Filters.Add(laa);

			config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
		}
	}
}