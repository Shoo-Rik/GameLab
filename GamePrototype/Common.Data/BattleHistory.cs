using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot("BattleHistory")]
    public class BattleHistory
    {
        [XmlElement("B")]
        public Battle[] Battles { get; set; }
    }
}
