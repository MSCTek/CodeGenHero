using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MSC.CodeGenHero.Serialization
{
    public static class Extensions
    {
        /// <summary>
        ///     Deserialize json string to the specified type
        /// </summary>
        /// <typeparam name="T"> Object type </typeparam>
        /// <param name="json"> Json string </param>
        /// <returns> Object representation of the json </returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        /// <summary>
        ///     Deserialize json stream to the specified type
        /// </summary>
        /// <typeparam name="T"> Object type </typeparam>
        /// <param name="jsonStream"> Json stream </param>
        /// <returns> Object representation of the json </returns>
        public static T FromJson<T>(this Stream jsonStream)
        {
            using (var reader = new StreamReader(jsonStream, Encoding.UTF8))
            {
                var jsonString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            }
        }

        /// <summary>
        ///     Deserialize json string to the specified type
        /// </summary>
        /// <param name="type"> Object type </param>
        /// <param name="json"> Json string </param>
        /// <returns> Object representation of the json </returns>
        public static object FromJson(this string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        /// <summary>
        ///     Converts a stream to byte array
        /// </summary>
        /// <param name="stream">Stream to convert</param>
        /// <returns>Stream content as byte array</returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        ///     Serialize object to json
        /// </summary>
        /// <param name="objectToSerialize"> Object to serialize </param>
        /// <returns> Json string </returns>
        public static string ToJson(this object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Newtonsoft.Json.Formatting.Indented });
        }
    }
}