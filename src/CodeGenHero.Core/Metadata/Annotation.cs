// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class Annotation : IAnnotation
	{
		public string Name { get; set; }

		public object Value { get; set; }
	}
}