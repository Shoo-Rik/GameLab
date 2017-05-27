using System;
using System.Drawing;
using Common.Data;
using System.Xml.Serialization;

namespace GamePrototype.Data
{
    [XmlRoot]
    public class XmlMapInfo
    {
        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("length")]
        public int Length { get; set; }

        [XmlAttribute("color")]
        public string ColorName { get; set; }

        [XmlIgnore]
        public Color Color
        {
            get
            {
                if (String.IsNullOrEmpty(ColorName))
                    return Color.White;

                return Color.FromName(ColorName);
            }
            set
            {
                ColorName = value.Name;
            }
        }

        [XmlAttribute("step")]
        public int Step { get; set; }

        [XmlElement("Info")]
        public RegionInformation[] Info { get; set; }

        [XmlArray("BattleHistory")]
        [XmlArrayItem("B")]
        public Battle[] Battles { get; set; }
    }

}
