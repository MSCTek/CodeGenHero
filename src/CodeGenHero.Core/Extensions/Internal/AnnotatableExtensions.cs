// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System.Linq;
using System.Text;
using JetBrains.Annotations;
using CodeGenHero.Core.Metadata;

namespace CodeGenHero.Core.Metadata.Internal
{
	public static class AnnotatableExtensions
	{
		public static string AnnotationsToDebugString([NotNull] this IAnnotatable annotatable, [NotNull] string indent = "")
		{
			var annotations = annotatable.GetAnnotations().ToList();
			if (annotations.Count == 0)
			{
				return "";
			}

			var builder = new StringBuilder();

			builder.AppendLine().Append(indent).Append("Annotations: ");
			foreach (var annotation in annotations)
			{
				builder
					.AppendLine()
					.Append(indent)
					.Append("  ")
					.Append(annotation.Name)
					.Append(": ")
					.Append(annotation.Value);
			}

			return builder.ToString();
		}
	}
}