// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class MetadataSourceProperties : IMetadataSourceProperties
	{
		/// <summary>
		/// A case-insensitive dictionary of key value pairs.
		/// </summary>
		public Dictionary<string, string> KeyValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public IList<AssemblyName> ReferencedAssemblies { get; set; } = new List<AssemblyName>();
	}
}