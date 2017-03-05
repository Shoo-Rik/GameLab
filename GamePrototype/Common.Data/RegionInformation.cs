using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot("RI")]
    public struct RegionInformation
    {
        [XmlAttribute("id")]
        public int LandId { get; set; }

        [XmlElement("Army")]
        public Army Army { get; set; }

        [XmlElement("Reserve")]
        public Reserve Reserve { get; set; }
    }
}
