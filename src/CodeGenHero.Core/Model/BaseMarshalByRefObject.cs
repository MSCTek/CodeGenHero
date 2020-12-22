using System;

namespace CodeGenHero.Core
{
    /// <summary>
    /// Basically tells .NET not to dispose of and GC our remote proxy objects.
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/2410221/appdomain-and-marshalbyrefobject-life-time-how-to-avoid-remotingexception"/>
    /// <seealso cref="https://social.msdn.microsoft.com/Forums/en-US/3ab17b40-546f-4373-8c08-f0f072d818c9/remotingexception-when-raising-events-across-appdomains?forum=netfxremoting"/>
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/ms973857.aspx"/>
    public abstract class BaseMarshalByRefObject : MarshalByRefObject
    {
        /// <summary>
        /// Basically tells .NET not to dispose of and GC our remote proxy objects.
        /// </summary>
        /// <remarks>
        /// Avoids "System.Runtime.Remoting.RemotingException: 'Object '*.rem' has been disconnected or does not exist at the server.'" errors.
        /// i.e. 'SelectedTemplate.Description' threw an exception of type 'System.Runtime.Remoting.RemotingException'
        /// </remarks>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}