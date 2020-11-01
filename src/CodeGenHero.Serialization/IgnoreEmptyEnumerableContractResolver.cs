using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MSC.CodeGenHero.Serialization
{
	/// <summary>
	/// Use to skip serializing empty lists.
	/// </summary>
	/// <see cref="https://stackoverflow.com/questions/11320968/can-newtonsoft-json-net-skip-serializing-empty-lists"/>
	public class IgnoreEmptyEnumerableContractResolver : CamelCasePropertyNamesContractResolver
	{
		public static readonly IgnoreEmptyEnumerableContractResolver Instance = new IgnoreEmptyEnumerableContractResolver();

		protected override JsonProperty CreateProperty(MemberInfo member,
			MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);

			if (property.PropertyType != typeof(string) &&
				typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
			{
				property.ShouldSerialize = instance =>
				{
					IEnumerable enumerable = null;
					// this value could be in a public field or public property
					switch (member.MemberType)
					{
						case MemberTypes.Property:
							enumerable = instance
								.GetType()
								.GetProperty(member.Name)
								?.GetValue(instance, null) as IEnumerable;
							break;

						case MemberTypes.Field:
							enumerable = instance
								.GetType()
								.GetField(member.Name)
								.GetValue(instance) as IEnumerable;
							break;
					}

					// If the list is null, we defer the decision to NullValueHandling setting in the serializer settings.
					return enumerable == null || enumerable.GetEnumerator().MoveNext(); // Using MoveNext is more performant than using a "Count", which has to traverse the entire collection.
				};
			}

			return property;
		}
	}
}