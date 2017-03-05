using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Region
    {
        // A2
        public Reserve Reserve { get; set; }

        // A10
        public Coordinates Coordinates { get; set; }
    }
}
