using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot("RI")]
    public class RegionInformation
    {
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
