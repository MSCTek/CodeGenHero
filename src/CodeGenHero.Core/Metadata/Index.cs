// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class Index : MetadataBase, IIndex
	{
		public IEntityType DeclaringEntityType { get; set; }

		public bool IsUnique { get; set; }

		public IList<IProperty> Properties { get; set; } = new List<IProperty>();
	}
}