// Copyright (c) Micro Support Center, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenHero.Core.Extensions
{
	public static class TypeExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="type"></param>
		/// <see cref="https://stackoverflow.com/questions/358835/getproperties-to-return-all-properties-for-an-interface-inheritance-hierarchy"/>
		/// <returns></returns>
		public static PropertyInfo[] GetPublicProperties(this Type type)
		{
			PropertyInfo[] retVal = null;

			if (type.IsInterface)
			{
				var propertyInfos = new List<PropertyInfo>();

				var considered = new List<Type>();
				var queue = new Queue<Type>();
				considered.Add(type);
				queue.Enqueue(type);

				while (queue.Count > 0)
				{
					var subType = queue.Dequeue();
					foreach (var subInterface in subType.GetInterfaces())
					{
						if (considered.Contains(subInterface)) continue;

						considered.Add(subInterface);
						queue.Enqueue(subInterface);
					}

					var typeProperties = subType.GetProperties(
						BindingFlags.FlattenHierarchy
						| BindingFlags.Public
						| BindingFlags.Instance);

					var newPropertyInfos = typeProperties
						.Where(x => !propertyInfos.Contains(x));

					propertyInfos.InsertRange(0, newPropertyInfos);
				}

				retVal = propertyInfos.ToArray();
			}
			else
			{
				retVal = type.GetProperties(BindingFlags.FlattenHierarchy
					| BindingFlags.Public | BindingFlags.Instance);
			}

			return retVal;
		}

		/// <summary>
		/// Determines whether the given type is "simple" or not complex.
		/// </summary>
		/// <param name="type">The value to check.</param>
		/// <returns>
		///   <c>true</c> if the specified <paramref name="type"/> is not a complex type; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsSimple(this Type type)
		{
			return type.IsPrimitive || type.Equals(typeof(string));
		}
	}
}