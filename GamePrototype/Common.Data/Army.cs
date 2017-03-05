using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Army
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        // A6
        [XmlElement("Region")]
        public Region CurrentRegion { get; set; }

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
