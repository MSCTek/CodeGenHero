// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class EntityType : MetadataBase, IEntityType
	{
		public Lazy<IList<IForeignKey>> ForeignKeyList = null;
		public Lazy<IList<IKey>> KeyList = null;
		public Lazy<IList<IProperty>> PropertyList = null;

		public EntityType()
		{
			ForeignKeyList = new Lazy<IList<IForeignKey>>(() => PopulateForeignKeys());
			PropertyList = new Lazy<IList<IProperty>>(() => PopulatePropertyList());
			KeyList = new Lazy<IList<IKey>>(() => PopulateKeyList());
		}

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
			IForeignKey retVal = null;

			foreach (var foreignKey in ForeignKeyList.Value)
			{
				if (PropertyListComparer.Instance.Equals(foreignKey.PrincipalKey.Properties, principalKey.Properties)
					&& StringComparer.Ordinal.Equals(foreignKey.PrincipalEntityType.Name, principalEntityType.Name))
				{
					retVal = foreignKey;
					break;
				}
			}

			return null;
		}

		public IKey FindKey([NotNull] IList<IProperty> properties)
		{
			IKey retVal = null;

			foreach (var keyValuePair in Keys)
			{
				if (PropertyListComparer.Instance.Compare(keyValuePair.Key, properties) == 0)
				{
					retVal = keyValuePair.Value;
				}

				//int numMatches = 0;
				//foreach (var parameterPropertyItem in properties)
				//{
				//	var matchingPropertyItemInKey = keyValuePair.Key.FirstOrDefault(x => x.Name == parameterPropertyItem.Name
				//	&& x.DeclaringEntityType.Name == parameterPropertyItem.DeclaringEntityType.Name);
				//	if (matchingPropertyItemInKey != null)
				//	{
				//		numMatches++;
				//	}
				//}

				//if (numMatches > 0 && numMatches == properties.Count)
				//{
				//	retVal = keyValuePair.Value;
				//}
			}

			return retVal;
		}

		public IKey FindPrimaryKey()
		{
			IKey retVal = KeyList.Value.FirstOrDefault();
			return retVal;
		}

		public IProperty FindProperty([NotNull] string name)
		{
			IProperty retVal = PropertyList.Value.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			return retVal;
		}

		public IList<IForeignKey> GetForeignKeys()
		{
			return ForeignKeyList.Value;
		}

		public IList<IKey> GetKeys()
		{
			return KeyList.Value;
		}

		public IList<IProperty> GetProperties()
		{
			return PropertyList.Value;
		}

		public IList<IForeignKey> PopulateForeignKeys()
		{
			var retVal = new List<IForeignKey>();

			foreach (var foreignKey in ForeignKeys)
			{
				retVal.Add(foreignKey);
			}

			return retVal;
		}

		public IList<IKey> PopulateKeyList()
		{
			var retVal = new List<IKey>();

			foreach (var kvp in Keys)
			{
				retVal.Add(kvp.Value);
			}

			return retVal;
		}

		private IList<IProperty> PopulatePropertyList()
		{
			IList<IProperty> retVal = new List<IProperty>();

			foreach (var kvp in Properties)
			{
				retVal.Add(kvp.Value);
			}

			return retVal;
		}
	}
}