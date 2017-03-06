using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Land
    {
        // A1
        public RegionInformation[] Regions { get; set; }

        // A3
        public Army[] Armies { get; set; }
    }
}
