// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System.Collections.Generic;
using System.Reflection;

namespace CodeGenHero.Core.Metadata
{
	public interface IMetadataSourceProperties
	{
		Dictionary<string, string> KeyValues { get; set; }

		IList<AssemblyName> ReferencedAssemblies { get; set; }
	}
}