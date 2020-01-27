// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	/// <summary>
	///     <para>
	///         A class that exposes annotations. Annotations allow for arbitrary metadata to be stored on an object.
	///     </para>
	/// </summary>
	public interface IAnnotatable
	{
		IList<KeyValuePair<string, IAnnotation>> Annotations { get; set; }

		/// <summary>
		///     Gets the value annotation with the given name, returning <c>null</c> if it does not exist.
		/// </summary>
		/// <param name="name"> The name of the annotation to find. </param>
		/// <returns>
		///     The value of the existing annotation if an annotation with the specified name already exists. Otherwise, <c>null</c>.
		/// </returns>
		//object this[[NotNull] string name] { get; }

		/// <summary>
		///     Gets the annotation with the given name, returning <c>null</c> if it does not exist.
		/// </summary>
		/// <param name="name"> The name of the annotation to find. </param>
		/// <returns>
		///     The existing annotation if an annotation with the specified name already exists. Otherwise, <c>null</c>.
		/// </returns>
		IAnnotation FindAnnotation([NotNull] string name);

		/// <summary>
		///     Gets all annotations on the current object.
		/// </summary>
		IList<IAnnotation> GetAnnotations();
	}
}