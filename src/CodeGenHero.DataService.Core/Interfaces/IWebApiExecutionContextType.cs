namespace CodeGenHero.DataService
{
	public interface IWebApiExecutionContextType
	{
		int Base { get; }
		int Current { get; set; }

		bool Equals(object other);

		bool Equals(WebApiExecutionContextType other);

		int GetHashCode();
	}
}