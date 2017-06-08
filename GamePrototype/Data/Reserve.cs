using System.Xml.Serialization;

namespace Data
{
    [XmlRoot]
    public class Reserve
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        // A9
        public Army CreateArmy()
        {
            // [TODO]
            return null;
        }
    }
}
