using System;

namespace MSC.BingoBuzz.Constants
{
	public static class Enums
	{
		[Flags]
		public enum WebApiExecutionContextType
		{
			Base = 1,
			BB = 2,
		}
	}
}