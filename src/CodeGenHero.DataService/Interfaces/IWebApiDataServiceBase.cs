using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGenHero.DataService
{
    public partial interface IWebApiDataServiceBase
    {
        HttpClient HttpClient { get; set; }

        string IsServiceOnlineRelativeUrl { get; set; }

        ILogger Log { get; set; }

        Task<bool> IsServiceOnlineAsync();
    }
}