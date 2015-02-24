using System.Xml.Serialization;

namespace Beem.LastFMClient
{
    [XmlRoot("lfm")]
    public class LastFmAuthResponse
    {
        [XmlElement("session")]
        public LastFmSession Session { get; set; }
    }
}
