using static CodeGenHero.EAMVCXamPOCO.DataService.Constants.Enums;

namespace CodeGenHero.EAMVCXamPOCO.DataService.Interface
{
	public interface IWebApiExecutionContext
    {
        string BaseFileUrl { get; }

        string BaseWebApiUrl { get; }

        string ConnectionIdentifier { get; }

        WebApiExecutionContextType ExecutionContextType { get; set; }
    }
}