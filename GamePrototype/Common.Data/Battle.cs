using System.Drawing;
using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Battle
    {
        [XmlAttribute("step")]
        public int Step { get; set; }

        [XmlAttribute("result")]
        public BattleResult Result { get; set; }

        [XmlIgnore]
        public Point From
        {
            get { return (Defender != null) ? Defender.From : new Point(); }
        }

        [XmlAttribute("a_dmg")]
        public int AttackerDamage { get; set; }

        [XmlAttribute("d_dmg")]
        public int DefenderDamage { get; set; }

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
