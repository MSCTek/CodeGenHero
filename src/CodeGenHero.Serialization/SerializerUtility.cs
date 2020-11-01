using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MSC.CodeGenHero.Serialization
{
	public static class SerializerUtility
	{
		private readonly static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
		{   // Used to optimize/reduce size of JSON output: https://www.newtonsoft.com/json/help/html/ReducingSerializedJSONSize.htm
			Formatting = Formatting.None,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			ContractResolver = IgnoreEmptyEnumerableContractResolver.Instance,
		};

		private readonly static YamlDotNet.Serialization.IDeserializer _yamlDeserializer = new YamlDotNet.Serialization.DeserializerBuilder()
			.WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance)
			.Build();

		private readonly static YamlDotNet.Serialization.ISerializer _yamlSerializer = new YamlDotNet.Serialization.SerializerBuilder()
					.WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance)
			.WithEmissionPhaseObjectGraphVisitor(args => new YamlIEnumerableSkipEmptyObjectGraphVisitor(args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer))
			.Build();

		public static T DeserializeJson<T>(Stream s) where T : class
		{
			T retVal = null;
			JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

			s.Position = 0;
			using (StreamReader reader = new StreamReader(s))
			{
				string json = reader.ReadToEnd();
				retVal = json.FromJson<T>();
			}

			return retVal;
		}

		public static T DeserializeObject<T>(string json)
		{
			var retVal = JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
			return retVal;
		}

		public static T DeserializeYaml<T>(Stream s) where T : class
		{
			T retVal = null;

			s.Position = 0;
			using (StreamReader reader = new StreamReader(s))
			{
				string yaml = reader.ReadToEnd();
				retVal = _yamlDeserializer.Deserialize<T>(yaml);
			}

			return retVal;
		}

		public static string JsonPrettyPrint(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return string.Empty;

			json = json.Replace(Environment.NewLine, "").Replace("\t", "");

			StringBuilder sb = new StringBuilder();
			bool quote = false;
			bool ignore = false;
			int offset = 0;
			int indentLength = 3;

			foreach (char ch in json)
			{
				switch (ch)
				{
					case '"':
						if (!ignore) quote = !quote;
						break;

					case '\'':
						if (quote) ignore = !ignore;
						break;
				}

				if (quote)
					sb.Append(ch);
				else
				{
					switch (ch)
					{
						case '{':
						case '[':
							sb.Append(ch);
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', ++offset * indentLength));
							break;

						case '}':
						case ']':
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', --offset * indentLength));
							sb.Append(ch);
							break;

						case ',':
							sb.Append(ch);
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', offset * indentLength));
							break;

						case ':':
							sb.Append(ch);
							sb.Append(' ');
							break;

						default:
							if (ch != ' ') sb.Append(ch);
							break;
					}
				}
			}

			return sb.ToString().Trim();
		}

		public static void SerializeJsonToStream(object value, Stream s)
		{
			JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

			using (StreamWriter writer = new StreamWriter(s))
			using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
			{
				serializer.Serialize(jsonWriter, value);
				jsonWriter.Flush();
			}
		}

		public static string ToJson(object objectToSerialize)
		{
			var json = JsonConvert.SerializeObject(objectToSerialize, _jsonSerializerSettings);
			return json;
		}

		public static string ToYAML(object objectToSerialize)
		{
			string retVal = null;

			using (MemoryStream memStream = new MemoryStream())
			{
				using (var streamWriter = new StreamWriter(memStream))
				{
					_yamlSerializer.Serialize(streamWriter, objectToSerialize);
					streamWriter.Flush();
					// Convert stream to string
					memStream.Seek(0, SeekOrigin.Begin);
					StreamReader reader = new StreamReader(memStream);
					retVal = reader.ReadToEnd();
				}
			}

			return retVal;
		}
	}
}