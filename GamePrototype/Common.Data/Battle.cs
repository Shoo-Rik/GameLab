using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Battle
    {
        [XmlAttribute("step")]
        public int Step { get; set; }

        // A4
        [XmlElement]
        public Army Attacker { get; set; }

        // A4
        [XmlElement]
        public Army Defender { get; set; }

        /*
        // A5
        public Region RegionUnit { get; set; }
        */
    }
}
