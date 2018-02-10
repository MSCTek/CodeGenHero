using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CodeGenHero.EAMVCXamPOCO
{
	public class GuidBoolJsonConverter : JsonConverter
	{
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		public override bool CanConvert(Type objectType)
		{
			//GUID or bool that can be nullable
			Type t = Nullable.GetUnderlyingType(objectType);
			return (t == typeof(Guid) || objectType == typeof(Guid)) || (t == typeof(bool) || objectType == typeof(bool));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type t = Nullable.GetUnderlyingType(objectType);
			bool isGuid = (t == typeof(Guid) || objectType == typeof(Guid));
			bool isBool = (t == typeof(bool) || objectType == typeof(bool));

			if (isGuid)
			{
				if (reader.Value != null)
				{
					return Guid.Parse(reader.Value.ToString().ToUpper());
				}
				else
				{
					return Guid.Empty;
				}
			}
			if (isBool)
			{
				if (reader.Value != null)
				{
					return reader.Value.Equals("Y") || reader.Value.Equals("1") || reader.Value.Equals(true) ? true : false;
				}
				else
				{
					return false;
				}
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value != null)
			{
				if (value is Guid)
				{
					// Send GUIDs in this format "{guid}", i.e. add the {}
					Guid g = (Guid)value;
					string retVal = g.ToString("B").ToUpper();
					JToken token = JToken.FromObject(retVal);
					token.WriteTo(writer);
				}
				if (value is bool)
				{
					// Send bool in this format "Y" or "N"
					bool b = (bool)value;
					JToken token = JToken.FromObject(b == true ? "Y" : "N");
					token.WriteTo(writer);
				}
			}
		}
	}
}