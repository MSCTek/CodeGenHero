// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using CodeGenHero.Core.Metadata;

namespace CodeGenHero.Core
{
	public class EntityTypePathComparer : IComparer<IEntityType>
	{
		public static readonly EntityTypePathComparer Instance = new EntityTypePathComparer();

		private EntityTypePathComparer()
		{
		}

		public virtual int Compare(IEntityType x, IEntityType y)
		{
			var result = StringComparer.Ordinal.Compare(x.Name, y.Name);
			if (result != 0)
			{
				return result;
			}

			while (true)
			{
				var xDefiningNavigationName = x.DefiningNavigationName;
				var yDefiningNavigationName = y.DefiningNavigationName;

				if (xDefiningNavigationName == null
					&& yDefiningNavigationName == null)
				{
					return StringComparer.Ordinal.Compare(x.Name, y.Name);
				}

				if (xDefiningNavigationName == null)
				{
					return -1;
				}

				if (yDefiningNavigationName == null)
				{
					return 1;
				}

				result = StringComparer.Ordinal.Compare(xDefiningNavigationName, yDefiningNavigationName);
				if (result != 0)
				{
					return result;
				}

				x = x.DefiningEntityType;
				y = y.DefiningEntityType;
			}
		}

		public virtual int GetHashCode([NotNull] IEntityType entityType)
		{
			var result = 0;
			while (true)
			{
				result = (result * 397)
						 ^ StringComparer.Ordinal.GetHashCode(entityType.Name);
				var definingNavigationName = entityType.DefiningNavigationName;
				if (definingNavigationName == null)
				{
					return result;
				}

				result = (result * 397)
						 ^ StringComparer.Ordinal.GetHashCode(definingNavigationName);
				entityType = entityType.DefiningEntityType;
			}
		}
	}
}