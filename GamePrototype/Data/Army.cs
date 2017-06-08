using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Data
{
    [XmlRoot]
    public class Army
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

        [XmlAttribute("count")]
        public int Count { get; set; }

        // A6
        [XmlElement("From")]
        public Coordinates From { get; set; }

        // A11
        [XmlAttribute("lastStep")]
        public long LastStep { get; set; }

        // A7
        public static Army Join(params Army[] armies)
        {
            // [TODO]
            return null;
        }

        // A8
        public Army[] Split(int count)
        {
            // [TODO]
            return null;
        }
    }
}
