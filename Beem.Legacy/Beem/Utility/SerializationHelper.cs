using Beem.Core.Models;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;

namespace Beem.Utility
{
    public class SerializationHelper
    {
        public static T GetObjectFromString<T>(string xmlContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StringReader(xmlContent);
            T result = (T)serializer.Deserialize(reader);
            return result;
        }

        public static KeyContainer GetKeys(string name)
        {
            XDocument keyDocument = XDocument.Load(name);

            KeyContainer container = new KeyContainer();
            container.DiFmPremiumKey = (from c in keyDocument.Root.Elements() where c.Attribute("type").Value == "difm" 
                                        select c).FirstOrDefault().Attribute("value").Value;
            container.LastFmKey = (from c in keyDocument.Root.Elements()
                                        where c.Attribute("type").Value == "lastfm"
                                        select c).FirstOrDefault().Attribute("value").Value;
            container.MsaKey = (from c in keyDocument.Root.Elements()
                                        where c.Attribute("type").Value == "msa"
                                        select c).FirstOrDefault().Attribute("value").Value;
            container.ZumoKey = (from c in keyDocument.Root.Elements()
                                        where c.Attribute("type").Value == "zumo"
                                        select c).FirstOrDefault().Attribute("value").Value;
            container.ZumoUrl = (from c in keyDocument.Root.Elements()
                                 where c.Attribute("type").Value == "zumourl"
                                 select c).FirstOrDefault().Attribute("value").Value;
            container.LastFmSecret = (from c in keyDocument.Root.Elements()
                                      where c.Attribute("type").Value == "lastfmsecret"
                                      select c).FirstOrDefault().Attribute("value").Value;

            return container;
        }
    }
}
