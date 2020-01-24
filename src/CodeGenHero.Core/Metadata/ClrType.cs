// Copyright (c) Micro Support Center, Inc. All rights reserved.

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Core.Metadata
{
	public class ClrType
	{
		public string Assembly { get; set; }
		public string FullName { get; set; }
		public string Name { get; set; }
		public string Namespace { get; set; }
	}
}