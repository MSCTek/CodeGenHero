using System.Net.Http.Headers;

namespace CodeGenHero.DataService
{
	/// <summary>
	/// Provides a layer of abstraction for other, more specific, data services.
	/// </summary>
	public interface IDataService<T>
	{
		T Instance { get; }

		string GetExecutionContexts();

		void SetDefaultHttpClientValues(AuthenticationHeaderValue defaultAuthenticationHeaderValue, int defaultRequestedVersion);

		void SetExecutionContexts(string executionContextsSerialized);
	}
}