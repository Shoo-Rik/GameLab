using System.Xml.Serialization;

namespace Data
{
    [XmlRoot]
    public class Battle
    {
        [XmlAttribute("step")]
        public int Step { get; set; }

        [XmlAttribute("result")]
        public BattleResult Result { get; set; }

        [XmlIgnore]
        public Coordinates From
        {
            get { return (Defender != null) ? Defender.From : new Coordinates(); }
        }

        [XmlAttribute("a_dmg")]
        public int DamageToAttacker { get; set; }

        [XmlAttribute("d_dmg")]
        public int DamageToDefender { get; set; }

        // A4
        [XmlElement("A")]
        public Army Attacker { get; set; }

        // A4
        [XmlElement("D")]
        public Army Defender { get; set; }

        [XmlElement("R")]
        public Reserve DefenderReserve { get; set; }

        /*
        // A5
        public Region RegionUnit { get; set; }
        */
    }
}
