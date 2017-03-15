using Common.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GamePrototype.Models
{
    public static class MapProcessor
    {
        #region [TODO]: Move to config

        private static readonly int DefaultArmyCount = 1000;
        private static readonly int ArmyPercentVariation = 25;

        private static readonly int DefaultReserveCount = 2000;
        private static readonly int ReservePercentVariation = 35;

        private static readonly int SelectedRegionColor = Color.DarkSlateGray.ToArgb();
        private static readonly int DisabledRegionColor = Color.Gray.ToArgb();

        private static int RegionSize = 64;

        #endregion [TODO]: Move to config

        internal static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        public static MapInfo InitializeMapInfo(int color)
        {
            var result = new MapInfo { OwnColor = color };

            var color1 = Color.Red.ToArgb();
            var color2 = Color.Green.ToArgb();
            var color3 = Color.Blue.ToArgb();

            const int widthCount = 10;
            const int heightCount = 10;
            result.Info = new RegionInformation[widthCount, heightCount];

            for (int wIndex = 0; wIndex < widthCount; ++wIndex)
            {
                for (int hIndex = 0; hIndex < heightCount; ++hIndex)
                {
                    switch (Rnd.Next(3))
                    {
                        case 0:
                            result.Info[wIndex, hIndex] = new RegionInformation { LandId = color1 };
                            break;

                        case 1:
                            result.Info[wIndex, hIndex] = new RegionInformation { LandId = color2 };
                            break;

                        case 2:
                            result.Info[wIndex, hIndex] = new RegionInformation { LandId = color3 };
                            break;

                        default:
                            throw new NotSupportedException("Random value");
                    }

                    result.Info[wIndex, hIndex].Coordinates = new Coordinates { X = wIndex, Y = hIndex };

                    int armyCount = DefaultArmyCount * (100 - ArmyPercentVariation + Rnd.Next(2 * ArmyPercentVariation)) / 100;
                    result.Info[wIndex, hIndex].Army = new Army
                    {
                        Count = armyCount,
                        LandId = result.Info[wIndex, hIndex].LandId
                    };

                    int reserveCount = DefaultReserveCount * (100 - ReservePercentVariation + Rnd.Next(2 * ReservePercentVariation)) / 100;
                    result.Info[wIndex, hIndex].Reserve = new Reserve
                    {
                        Count = reserveCount
                    };
                }
            }
            return result;
        }

        // [TODO]: Refactor
        public static Bitmap GenerateMap(MapInfo mapInfo, Point selectedLocation, GameMode mode)
        {
            int widthCount = mapInfo.Info.GetLength(0);
            int heightCount = mapInfo.Info.GetLength(1);

            int wSelectedIndex = GetWIndex(selectedLocation);
            int hSelectedIndex = GetHIndex(selectedLocation);

            Bitmap bmp = new Bitmap(RegionSize * widthCount, RegionSize * heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            for (int wIndex = 0; wIndex < widthCount; ++wIndex)
            {
                for (int hIndex = 0; hIndex < heightCount; ++hIndex)
                {
                    bool hasBattle = false;
                    int colorId;
                    if (wSelectedIndex == wIndex && hSelectedIndex == hIndex)
                    {
                        colorId = SelectedRegionColor;
                    }
                    else if (!((mode == GameMode.Normal) ||
                               (mode == GameMode.Attack || mode == GameMode.Relocation) && (Math.Abs(hIndex - hSelectedIndex) <= 1) &&
                               (Math.Abs(wIndex - wSelectedIndex) <= 1)))
                    {
                        colorId = DisabledRegionColor;
                    }
                    else
                    {
                        colorId = mapInfo.Info[wIndex, hIndex].LandId;
                        hasBattle = mapInfo.Info[wIndex, hIndex].Battles != null &&
                                    mapInfo.Info[wIndex, hIndex].Battles.Length > 0;
                    }

                    const int shift = 4;
                    int count = 0;

                    if (hasBattle)
                    {
                        foreach (var battle in mapInfo.Info[wIndex, hIndex].Battles)
                        {
                            gr.FillRectangle(CreateBrush(battle.Attacker.LandId),
                                new Rectangle(
                                    wIndex * RegionSize + count * shift,
                                    hIndex * RegionSize + count * shift,
                                    RegionSize - 1 - 2 * count * shift,
                                    RegionSize - 1 - 2 * count * shift));
                            ++count;
                        }
                    }

                    gr.FillRectangle(CreateBrush(colorId),
                        new Rectangle(
                            wIndex * RegionSize + count * shift,
                            hIndex * RegionSize + count * shift,
                            RegionSize - 1 - 2 * count * shift,
                            RegionSize - 1 - 2 * count * shift));
                }
            }
            return bmp;
        }

        public static int GetWIndex(Point location)
        {
            return location.X == 0 ? -1 : location.X / RegionSize;
        }

        public static int GetHIndex(Point location)
        {
            return location.Y == 0 ? -1 : location.Y / RegionSize;
        }

        public static SolidBrush CreateBrush(int colorId)
        {
            return new SolidBrush(Color.FromArgb(colorId));
        }

        public static RegionInformation[] GetNearOwnRegions(MapInfo mapInfo, Coordinates coordinates, int landId)
        {
            var result = new List<RegionInformation>();

            int wMaxIndex = mapInfo.Info.GetLength(0);
            int hMaxIndex = mapInfo.Info.GetLength(1);

            for (int distance = 1; distance < Math.Max(wMaxIndex, hMaxIndex); ++distance)
            {
                if (result.Count > 0)
                    break;

                int wIndex = coordinates.X + distance;
                if (wIndex < 0 || wIndex >= wMaxIndex)
                    continue;

                int hIndex;
                for (int y = -distance; y <= distance; ++y)
                {
                    hIndex = coordinates.Y + y;
                    if (hIndex >= 0 && hIndex < hMaxIndex)
                    {
                        result.TryAddRegion(mapInfo, landId, wIndex, hIndex);
                    }
                }

                wIndex = coordinates.X - distance;
                if (wIndex < 0 || wIndex >= wMaxIndex)
                    continue;

                for (int y = -distance; y <= distance; ++y)
                {
                    hIndex = coordinates.Y + y;
                    if (hIndex >= 0 && hIndex < hMaxIndex)
                    {
                        result.TryAddRegion(mapInfo, landId, wIndex, hIndex);
                    }
                }

                hIndex = coordinates.Y + distance;
                if (hIndex < 0 || hIndex >= hMaxIndex)
                    continue;

                for (int x = -distance + 1; x < distance; ++x)
                {
                    wIndex = coordinates.X + x;
                    if (wIndex >= 0 && wIndex < wMaxIndex)
                    {
                        result.TryAddRegion(mapInfo, landId, wIndex, hIndex);
                    }
                }

                hIndex = coordinates.Y - distance;
                if (hIndex < 0 || hIndex >= hMaxIndex)
                    continue;

                for (int x = -distance + 1; x < distance; ++x)
                {
                    wIndex = coordinates.X + x;
                    if (wIndex >= 0 || wIndex < wMaxIndex)
                    {
                        result.TryAddRegion(mapInfo, landId, wIndex, hIndex);
                    }
                }
            }

            return result.ToArray();
        }

        private static void TryAddRegion(this ICollection<RegionInformation> result, MapInfo mapInfo, int targetLandId, int wIndex, int hIndex)
        {
            RegionInformation info = mapInfo.Info[wIndex, hIndex];
            if (info.LandId == targetLandId)
            {
                result.Add(info);
            }
        }
    }
}
