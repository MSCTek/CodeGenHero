// <auto-generated> - Template:APIStatusController, Version:1.0, Id:a5d78309-5a18-4a09-9225-03e602b52187
using System;
using System.Threading.Tasks;
using System.Web.Http;
using CodeGenHero.WebApi;
using CodeGenHero.Logging;
using CodeGenHero.BingoBuzz.Repository.Interface;

namespace CodeGenHero.BingoBuzz.API.Controllers.BB
{
	public partial class BBAPIStatusController : BBBaseApiController
	{
		public BBAPIStatusController() : base()
		{
		}

		public BBAPIStatusController(ILoggingService log, IBBRepository repository)
			: base(log, repository)
		{
		}

		[HttpGet]
		[VersionedRoute(template: "APIStatus", allowedVersion: 1, Name = "BBAPIStatus")]
		public async Task<IHttpActionResult> Get()
		{
			try
			{
				return Ok();
			}
			catch (Exception ex)
			{
				Error(message: ex.Message, logMessageType: LogMessageType.Instance.Exception_WebApi, ex: ex);

				if (System.Diagnostics.Debugger.IsAttached)
					System.Diagnostics.Debugger.Break();

				return InternalServerError();
			}
		}
	}
}