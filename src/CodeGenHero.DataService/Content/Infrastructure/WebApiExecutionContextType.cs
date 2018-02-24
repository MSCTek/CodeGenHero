using System;

namespace CodeGenHero.DataService
{
	public partial class WebApiExecutionContextType
	{
		private static readonly Lazy<WebApiExecutionContextType> _lazyInstance = new Lazy<WebApiExecutionContextType>(() => new WebApiExecutionContextType());

		private WebApiExecutionContextType()
		{
		}

		public static WebApiExecutionContextType Instance { get { return _lazyInstance.Value; } }

		public virtual int Base => 1;
	}
}