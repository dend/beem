using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Beem.Core.Models
{
    public class KeyContainer
    {
        public string DiFmPremiumKey { get; set; }
        public string LastFmKey { get; set; }
        public string ZumoKey { get; set; }
        public string MsaKey { get; set; }
    }
}
