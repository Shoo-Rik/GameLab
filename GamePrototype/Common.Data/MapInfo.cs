using System.Collections.Generic;

namespace Common.Data
{
    public class MapInfo
    {
        public MapInfo()
        {
            BattleHistory = new List<Battle>();
        }

        public int OwnColor { get; set; }

        public int Step { get; set; }

        public RegionInformation[,] Info { get; set; }

        public List<Battle> BattleHistory { get; set; }
    }
}
