using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace MSC.CodeGenHero.Serialization
{
	public sealed class YamlIEnumerableSkipEmptyObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		private readonly ObjectSerializer nestedObjectSerializer;
		private readonly IEnumerable<IYamlTypeConverter> typeConverters;

		public YamlIEnumerableSkipEmptyObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEnumerable<IYamlTypeConverter> typeConverters, ObjectSerializer nestedObjectSerializer)
			: base(nextVisitor)
		{
			this.typeConverters = typeConverters != null
				? typeConverters.ToList()
				: Enumerable.Empty<IYamlTypeConverter>();

			this.nestedObjectSerializer = nestedObjectSerializer;
		}

		public override bool Enter(IObjectDescriptor value, IEmitter context)
		{
			bool retVal;

			if (typeof(System.Collections.IEnumerable).IsAssignableFrom(value.Value.GetType()))
			{   // We have a collection
				var enumerableObject = (System.Collections.IEnumerable)value.Value;
				if (enumerableObject.GetEnumerator().MoveNext()) // Returns true if the collection is not empty.
				{   // Serialize it as normal.
					retVal = base.Enter(value, context);
				}
				else
				{   // Skip this item.
					retVal = false;
				}
			}
			else
			{   // Not a collection, normal serialization.
				retVal = base.Enter(value, context);
			}

			return retVal;
		}

		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			bool retVal = false;

			if (value.Value == null)
				return retVal;

			if (typeof(System.Collections.IEnumerable).IsAssignableFrom(value.Value.GetType()))
			{   // We have a collection
				var enumerableObject = (System.Collections.IEnumerable)value.Value;
				if (enumerableObject.GetEnumerator().MoveNext()) // Returns true if the collection is not empty.
				{   // Don't skip this item - serialize it as normal.
					retVal = base.EnterMapping(key, value, context);
				}
				// Else we have an empty collection and the initialized return value of false is correct.
			}
			else
			{   // Not a collection, normal serialization.
				retVal = base.EnterMapping(key, value, context);
			}

			return retVal;
		}
	}
}