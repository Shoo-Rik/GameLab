using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot("RI")]
    public class RegionInformation
    {
        [XmlAttribute("id")]
        public int LandId { get; set; }

        [XmlElement("CO")]
        public Coordinates Coordinates { get; set; }

        [XmlElement("Army")]
        public Army Army { get; set; }

        // A2
        [XmlElement("Reserve")]
        public Reserve Reserve { get; set; }

        // A10
        // [TODO]

        [XmlElement("Battle")]
        public Battle[] Battles { get; set; }
    }
}
