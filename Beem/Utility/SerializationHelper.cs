using System.IO;
using System.Xml.Serialization;

namespace Beem.Utility
{
    public static class SerializationHelper
    {
        public static T GetObjectFromString<T>(string xmlContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StringReader(xmlContent);
            T result = (T)serializer.Deserialize(reader);
            return result;
        }
    }
}
