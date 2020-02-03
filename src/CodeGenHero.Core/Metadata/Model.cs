// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class Model : MetadataBase, IModel
	{
		public IList<IEntityType> EntityTypes { get; set; } = new List<IEntityType>();

		public string ModelFullyQualifiedTypeName { get; set; }

		public string ModelTypeName { get; set; }

		public IEntityType FindEntityType([NotNull] string name)
		{
			return EntityTypes.FirstOrDefault(x => x.Name == name);
		}

		public IEntityType FindEntityType([NotNull] string name, [NotNull] string definingNavigationName, [NotNull] IEntityType definingEntityType)
		{
			return EntityTypes.FirstOrDefault(x => x.Name == name && x.DefiningNavigationName == definingNavigationName && x.DefiningEntityType == x.DefiningEntityType);
		}

		public IList<IEntityType> GetEntityTypes()
		{
			return EntityTypes;
		}
	}
}