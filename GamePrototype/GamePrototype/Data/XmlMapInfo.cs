using Common.Data;
using System.Xml.Serialization;

namespace GamePrototype.Data
{
    [XmlRoot]
    public class XmlMapInfo
    {
        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int Length { get; set; }

        [XmlAttribute]
        public int Color { get; set; }

        [XmlAttribute]
        public int Step { get; set; }

        [XmlElement("Info")]
        public RegionInformation[] Info { get; set; }

        [XmlArray("BattleHistory")]
        [XmlArrayItem("B")]
        public Battle[] Battles { get; set; }
    }

}
