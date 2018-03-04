namespace CodeGenHero.DataService
{
	public interface IWebApiExecutionContext
	{
		string BaseFileUrl { get; }

		string BaseWebApiUrl { get; }

		string ConnectionIdentifier { get; }

		IWebApiExecutionContextType ExecutionContextType { get; set; }
	}
}