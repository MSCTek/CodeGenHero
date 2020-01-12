// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenHero.Core.Metadata
{
	public class Model : MetadataBase, IModel
	{
		public IList<IEntityType> EntityTypes { get; set; } = new List<IEntityType>();

		public IEntityType FindEntityType([NotNull] string name)
		{
			return EntityTypes.FirstOrDefault(x => x.Name == name);
		}

		public IEntityType FindEntityType([NotNull] string name, [NotNull] string definingNavigationName, [NotNull] IEntityType definingEntityType)
		{
			return EntityTypes.FirstOrDefault(x => x.Name == name && x.DefiningNavigationName == definingNavigationName && x.DefiningEntityType == x.DefiningEntityType);
		}

		public IEnumerable<IEntityType> GetEntityTypes()
		{
			return EntityTypes;
		}
	}
}