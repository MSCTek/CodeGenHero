using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

//using Newtonsoft.Json.Bson;
using System.IO;

namespace CodeGenHero.DataService
{
	public static class HttpExtensions
	{
		public static byte[] SerializeBson<T>(T obj)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BsonDataWriter writer = new BsonDataWriter(ms))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(writer, obj);
				}

				//string data = Convert.ToBase64String(ms.ToArray());
				return ms.ToArray();
			}
		}
	}
}