// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     Represents an entity type in an <see cref="IModel" />.
	/// </summary>
	public interface IEntityType : ITypeBase, IAnnotatable
	{
		/// <summary>
		///     Gets the base type of this entity type. Returns <c>null</c> if this is not a derived type in an inheritance hierarchy.
		/// </summary>
		IEntityType BaseType { get; set; }

		/// <summary>
		///     Gets the defining entity type.
		/// </summary>
		IEntityType DefiningEntityType { get; set; }

		/// <summary>
		///     Gets the name of the defining navigation.
		/// </summary>
		string DefiningNavigationName { get; set; }

		SortedSet<IForeignKey> ForeignKeys { get; set; }
		SortedDictionary<IList<IProperty>, IIndex> Indexes { get; set; }
		SortedDictionary<IList<IProperty>, IKey> Keys { get; set; }
		IList<INavigation> Navigations { get; set; }
		SortedDictionary<string, IProperty> Properties { get; set; }

		/// <summary>
		///     Gets the foreign key for the given properties that points to a given primary or alternate key.
		///     Returns <c>null</c> if no foreign key is found.
		/// </summary>
		/// <param name="properties"> The properties that the foreign key is defined on. </param>
		/// <param name="principalKey"> The primary or alternate key that is referenced. </param>
		/// <param name="principalEntityType">
		///     The entity type that the relationship targets. This may be different from the type that <paramref name="principalKey" />
		///     is defined on when the relationship targets a derived type in an inheritance hierarchy (since the key is defined on the
		///     base type of the hierarchy).
		/// </param>
		/// <returns> The foreign key, or <c>null</c> if none is defined. </returns>
		IForeignKey FindForeignKey(
			[NotNull] IList<IProperty> properties,
			[NotNull] IKey principalKey,
			[NotNull] IEntityType principalEntityType);

		/// <summary>
		///     Gets the primary or alternate key that is defined on the given properties.
		///     Returns <c>null</c> if no key is defined for the given properties.
		/// </summary>
		/// <param name="properties"> The properties that make up the key. </param>
		/// <returns> The key, or <c>null</c> if none is defined. </returns>
		IKey FindKey([NotNull] IList<IProperty> properties);

		/// <summary>
		///     Gets primary key for this entity. Returns <c>null</c> if no primary key is defined.
		/// </summary>
		/// <returns> The primary key, or <c>null</c> if none is defined. </returns>
		IKey FindPrimaryKey();

		/// <summary>
		///     <para>
		///         Gets the property with a given name. Returns <c>null</c> if no property with the given name is defined.
		///     </para>
		///     <para>
		///         This API only finds scalar properties and does not find navigation properties.
		///     </para>
		/// </summary>
		/// <param name="name"> The name of the property. </param>
		/// <returns> The property, or <c>null</c> if none is found. </returns>
		IProperty FindProperty([NotNull] string name);

		/// <summary>
		///     Gets the foreign keys defined on this entity.
		/// </summary>
		/// <returns> The foreign keys defined on this entity. </returns>
		IEnumerable<IForeignKey> GetForeignKeys();

		/// <summary>
		///     Gets the primary and alternate keys for this entity.
		/// </summary>
		/// <returns> The primary and alternate keys. </returns>
		IEnumerable<IKey> GetKeys();

		/// <summary>
		///     <para>
		///         Gets the properties defined on this entity.
		///     </para>
		///     <para>
		///         This API only returns scalar properties and does not return navigation properties.
		///     </para>
		/// </summary>
		/// <returns> The properties defined on this entity. </returns>
		IEnumerable<IProperty> GetProperties();
	}
}