// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	public class EntityType : MetadataBase, IEntityType
	{
		public IEntityType BaseType { get; set; }
		public Type ClrType { get; set; }
		public IEntityType DefiningEntityType { get; set; }
		public string DefiningNavigationName { get; set; }
		public SortedSet<IForeignKey> ForeignKeys { get; set; } = new SortedSet<IForeignKey>(ForeignKeyComparer.Instance);
		public SortedDictionary<IList<IProperty>, IIndex> Indexes { get; set; } = new SortedDictionary<IList<IProperty>, IIndex>(PropertyListComparer.Instance);
		public SortedDictionary<IList<IProperty>, IKey> Keys { get; set; } = new SortedDictionary<IList<IProperty>, IKey>(PropertyListComparer.Instance);

		//public IModel Model { get; set; }
		public string Name { get; set; }

		public IList<INavigation> Navigations { get; set; } = new List<INavigation>();
		public SortedDictionary<string, IProperty> Properties { get; set; } = new SortedDictionary<string, IProperty>(StringComparer.Ordinal);

		public IForeignKey FindForeignKey([NotNull] IList<IProperty> properties, [NotNull] IKey principalKey, [NotNull] IEntityType principalEntityType)
		{
			throw new NotImplementedException();
		}

		public IKey FindKey([NotNull] IList<IProperty> properties)
		{
			throw new NotImplementedException();
		}

		public IKey FindPrimaryKey()
		{
			throw new NotImplementedException();
		}

		public IProperty FindProperty([NotNull] string name)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IForeignKey> GetForeignKeys()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IKey> GetKeys()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IProperty> GetProperties()
		{
			throw new NotImplementedException();
		}
	}
}