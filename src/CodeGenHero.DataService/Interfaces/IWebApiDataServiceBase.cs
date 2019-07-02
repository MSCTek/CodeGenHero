using CodeGenHero.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
	public partial interface IWebApiDataServiceBase
	{
		HttpClient HttpClient { get; set; }

		string IsServiceOnlineRelativeUrl { get; set; }

		ILoggingService Log { get; set; }

		Task<bool> IsServiceOnlineAsync();
	}
}