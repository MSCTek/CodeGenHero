// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
	public class TypeBase : ITypeBase
	{
		public Type ClrType { get; set; }

		//public IModel Model { get; set; }
		public string Name { get; set; }
	}
}