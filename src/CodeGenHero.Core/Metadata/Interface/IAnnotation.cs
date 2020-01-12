// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     <para>
	///         An arbitrary piece of metadata that can be stored on an object that implements <see cref="IAnnotatable" />.
	///     </para>
	/// </summary>
	public interface IAnnotation
	{
		/// <summary>
		///     Gets the key of this annotation.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		///     Gets the value assigned to this annotation.
		/// </summary>
		object Value { get; set; }
	}
}