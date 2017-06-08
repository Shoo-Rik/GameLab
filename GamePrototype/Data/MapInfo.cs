using System.Collections.Generic;
using System.Drawing;

namespace Data
{
    public class MapInfo
    {
        public MapInfo()
        {
            BattleHistory = new List<Battle>();
        }

        public Color OwnColor { get; set; }

        public int Step { get; set; }

        public RegionInformation[,] Info { get; set; }

        public List<Battle> BattleHistory { get; set; }
    }
}
