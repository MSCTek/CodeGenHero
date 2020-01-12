// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     Represents a scalar property of an entity.
	/// </summary>
	public interface IProperty : IPropertyBase
	{
		/// <summary>
		///     Gets the entity type that this property belongs to.
		/// </summary>
		IEntityType DeclaringEntityType { get; set; }

		/// <summary>
		///     Gets a value indicating whether this property is used as a concurrency token. When a property is configured
		///     as a concurrency token the value in the database will be checked when an instance of this entity type
		///     is updated or deleted during <see cref="DbContext.SaveChanges()" /> to ensure it has not changed since
		///     the instance was retrieved from the database. If it has changed, an exception will be thrown and the
		///     changes will not be applied to the database.
		/// </summary>
		bool IsConcurrencyToken { get; set; }

		/// <summary>
		///     Gets a value indicating whether this property can contain <c>null</c>.
		/// </summary>
		bool IsNullable { get; set; }
	}
}