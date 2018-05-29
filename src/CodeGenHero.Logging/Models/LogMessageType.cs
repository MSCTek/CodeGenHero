using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.Logging
{
	public partial class LogMessageType
	{
		private static readonly Lazy<LogMessageType> _lazyInstance = new Lazy<LogMessageType>(() => new LogMessageType());

		private LogMessageType()
		{
		}

		public static LogMessageType Instance { get { return _lazyInstance.Value; } }

		public virtual int Authentication_Fail => 501;
		public virtual int Authentication_Info => 500;
		public virtual int Authentication_Success => 502;
		public virtual int Exception_Application => 101;
		public virtual int Exception_Authenticate => 111;
		public virtual int Exception_Database => 102;
		public virtual int Exception_Domain => 115;
		public virtual int Exception_General => 103;
		public virtual int Exception_Repository => 116;
		public virtual int Exception_Synchronization => 110;
		public virtual int Exception_Unhandled => 104;
		public virtual int Exception_WebApi => 105;
		public virtual int Exception_WebApiClient => 108;
		public virtual int ExcessiveImageSize => 1002;
		public virtual int Info_Diagnostics => 301;
		public virtual int Info_General => 300;
		public virtual int Info_Synchronization => 310;
		public virtual int Unauthorized => 401;
		public virtual int Unknown => 100;
		public virtual int Warn_Domain => 215;
		public virtual int Warn_Repository => 216;
		public virtual int Warn_Mobile => 206;
		public virtual int Warn_Synchronization => 210;
		public virtual int Warn_Web => 207;
		public virtual int Warn_WebApi => 205;
		public virtual int Warn_WebApiClient => 208;
		public virtual int WebApi_PathAndQuery => 1001;
	}
}