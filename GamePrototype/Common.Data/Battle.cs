using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Battle
    {
        // A4
        public Army Attacker { get; set; }
        public Army Defender { get; set; }

        /*
        // A5
        public Region RegionUnit { get; set; }
        */
    }
}
