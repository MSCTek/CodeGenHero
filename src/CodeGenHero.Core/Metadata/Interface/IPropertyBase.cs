// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     Base type for navigation and scalar properties.
	/// </summary>
	public interface IPropertyBase : IAnnotatable
	{
		/// <summary>
		///     Gets the type of value that this property holds.
		/// </summary>
		ClrType ClrType { get; set; }

		/// <summary>
		///     Gets the type that this property belongs to.
		/// </summary>
		ITypeBase DeclaringType { get; set; }

		/// <summary>
		///     Gets the name of the property.
		/// </summary>
		string Name { get; set; }
	}
}