using System.Xml.Serialization;

namespace Common.Data
{
    [XmlRoot]
    public class Region
    {
        public Region(Coordinates coordinates)
        {
            Coordinates = coordinates;

        }
        
        // A2
        public Reserve ReserveUnit { get; set; }

        // A10
        public Coordinates Coordinates { get; private set; }
    }
}
