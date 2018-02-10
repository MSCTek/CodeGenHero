using AutoMapper;
using CodeGenHero.EAMVCXamPOCO.Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MSC.BingoBuzz.API
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			System.Web.Http.GlobalConfiguration.Configuration.Formatters.Add(new System.Net.Http.Formatting.BsonMediaTypeFormatter());

			// Initialize Automapper
			AutoMapperInitializer.Initialize();
			Mapper.AssertConfigurationIsValid();
		}
	}
}