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

		// public Type ClrType { get; set; } // JSON.Net cannot deserialize an abstract class like "Type"
		public ClrType ClrType { get; set; }

		public IEntityType DefiningEntityType { get; set; }
		public string DefiningNavigationName { get; set; }
		public SortedSet<IForeignKey> ForeignKeys { get; set; } = new SortedSet<IForeignKey>(ForeignKeyComparer.Instance);

		public IList<KeyValuePair<IList<IProperty>, IIndex>> Indexes { get; set; } = new List<KeyValuePair<IList<IProperty>, IIndex>>();
		public IList<KeyValuePair<IList<IProperty>, IKey>> Keys { get; set; } = new List<KeyValuePair<IList<IProperty>, IKey>>();

		//public IModel Model { get; set; }
		public string Name { get; set; }

		public IList<INavigation> Navigations { get; set; } = new List<INavigation>();
		public IList<KeyValuePair<string, IProperty>> Properties { get; set; } = new List<KeyValuePair<string, IProperty>>();

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