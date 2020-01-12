// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     Represents an index on a set of properties.
	/// </summary>
	public interface IIndex : IAnnotatable
	{
		/// <summary>
		///     Gets the entity type the index is defined on. This may be different from the type that <see cref="Properties" />
		///     are defined on when the index is defined a derived type in an inheritance hierarchy (since the properties
		///     may be defined on a base type).
		/// </summary>
		IEntityType DeclaringEntityType { get; }

		/// <summary>
		///     Gets a value indicating whether the values assigned to the indexed properties are unique.
		/// </summary>
		bool IsUnique { get; }

		/// <summary>
		///     Gets the properties that this index is defined on.
		/// </summary>
		IList<IProperty> Properties { get; }
	}
}