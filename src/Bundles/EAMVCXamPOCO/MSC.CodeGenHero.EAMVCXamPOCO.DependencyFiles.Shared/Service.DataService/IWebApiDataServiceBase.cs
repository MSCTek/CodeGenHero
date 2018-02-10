using CodeGenHero.EAMVCXamPOCO.Interface;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenHero.EAMVCXamPOCO.DataService.Interface
{
	public partial interface IWebApiDataServiceBase
	{
		AuthenticationHeaderValue DefaultAuthenticationHeaderValue { get; set; }

		int DefaultRequestedVersion { get; set; }

		IWebApiExecutionContext ExecutionContext { get; set; }

		ILoggingService Log { get; set; }

		//HttpClient GetClient(int requestedVersion = 1);

		HttpClient GetClient(AuthenticationHeaderValue authorization, int requestedVersion, string connectionIdentifier);

		Task<bool> IsServiceOnlineAsync();
	}
}