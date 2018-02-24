using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.Logging
{
	public partial class WebApiAuthenticationHeaderValueType
	{
		private static readonly Lazy<WebApiAuthenticationHeaderValueType> _lazyInstance = new Lazy<WebApiAuthenticationHeaderValueType>(() => new WebApiAuthenticationHeaderValueType());

		private WebApiAuthenticationHeaderValueType()
		{
		}

		public static WebApiAuthenticationHeaderValueType Instance { get { return _lazyInstance.Value; } }

		public virtual int BasicAuth => 1;
		public virtual int BearerToken => 2;
	}
}