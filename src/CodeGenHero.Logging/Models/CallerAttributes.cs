// The following is to enable "", "", and "" when targeting the .NET 4.0 Framework
// See: https://www.thomaslevesque.com/2012/06/13/using-c-5-caller-info-attributes-when-targeting-earlier-versions-of-the-net-framework/#comments

#if NET40
namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class CallerMemberNameAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class CallerFilePathAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class CallerLineNumberAttribute : Attribute
	{
	}
}
#endif