using System.Collections.Generic;
using System.Reflection;

namespace CodeGenHero.Core.Metadata.Interfaces
{
    public interface IMetadataSourceProperties
    {
        Dictionary<string, string> KeyValues { get; set; }

        IList<AssemblyName> ReferencedAssemblies { get; set; }
    }
}