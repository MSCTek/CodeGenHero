// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	public class Annotation : IAnnotation
	{
		public string Name { get; set; }

		public object Value { get; set; }
	}
}