// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;

namespace CodeGenHero.Core.Metadata
{
	[Serializable]
	public class TypeBase : ITypeBase
	{
		public ClrType ClrType { get; set; }

		//public IModel Model { get; set; }
		public string Name { get; set; }
	}
}