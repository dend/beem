using System.Xml.Serialization;

namespace Beem.LastFMClient
{
    public class LastFmSession
    {
        [XmlElement("key")]
        public string Key { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("subscriber")]
        public int Subscriber { get; set; }
    }
}
