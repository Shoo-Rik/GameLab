using System.Xml.Serialization;

namespace Data
{
    [XmlRoot]
    public class Coordinates
    {
        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var o = obj as Coordinates;
            if (o == null)
                return false;

            return (X == o.X) && (Y == o.Y);
        }

        public override int GetHashCode()
        {
            return (X ^ Y).GetHashCode();
        }
    }
}
