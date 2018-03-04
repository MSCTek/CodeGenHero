using AutoMapper;
using CodeGenHero.Repository.AutoMapper;
using System.Web.Http;

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