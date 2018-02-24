using System;

namespace CodeGenHero.Logging
{
	public static class Enums
	{
		[Flags]
		public enum LogLevel
		{   // Based on log4net.Core.Level
			Off = 0,
			Fatal = 1,
			Error = 2,
			Warn = 4,
			Info = 8,
			Debug = 16,
			All = 31
		}
	}
}