namespace CodeGenHero.Logging
{
	public interface IWebApiAuthenticationHeaderValueType
	{
		int BasicAuth { get; }
		int BearerToken { get; }
	}
}