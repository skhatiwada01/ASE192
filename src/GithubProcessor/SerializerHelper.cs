using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GithubProcessor
{
    public static class SerializerHelper
    {
        public static string SerializeToXml<T>(this T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new MemoryStream())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static void SerializeToXmlFile<T>(this T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T DeserializeFromXmlFile<T>(string filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings {CheckCharacters = false};

            using (var reader = XmlReader.Create(filePath, settings))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
