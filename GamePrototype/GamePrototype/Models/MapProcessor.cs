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

                    result.Info[wIndex, hIndex].Coordinates = new Point(wIndex, hIndex);

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
        public static Bitmap GenerateMap(this MapInfo mapInfo, Point selectedLocation, GameMode mode)
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
                    else if (!mapInfo.Info.IsRegionFromTargetArea(mode, new Point(wSelectedIndex, hSelectedIndex), new Point(wIndex, hIndex)))
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

        public static RegionInformation[] GetNearOwnRegions(this RegionInformation[,] info, Point coordinates, int landId)
        {
            var result = new List<RegionInformation>();

            int wMaxIndex = info.GetLength(0);
            int hMaxIndex = info.GetLength(1);

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
                        result.TryAddRegion(info, landId, wIndex, hIndex);
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
                        result.TryAddRegion(info, landId, wIndex, hIndex);
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
                        result.TryAddRegion(info, landId, wIndex, hIndex);
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
                        result.TryAddRegion(info, landId, wIndex, hIndex);
                    }
                }
            }

            return result.ToArray();
        }

        // [TODO]: Using constant 1
        public static bool IsRegionAllowedToAttack(this RegionInformation[,] info, Point mouseClickLocation, Point selectedLocation, int ownColor)
        {
            int attackedWIndex = GetWIndex(mouseClickLocation);
            int attackedHIndex = GetHIndex(mouseClickLocation);
            int currentWIndex = GetWIndex(selectedLocation);
            int currentHIndex = GetHIndex(selectedLocation);

            RegionInformation attackedRegion = info.GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1)
                && attackedRegion != null && (attackedRegion.LandId != ownColor));
        }

        public static bool IsRegionAllowedToRelocation(this RegionInformation[,] info, Point mouseClickLocation, Point selectedLocation)
        {
            int wSelectedIndex = GetWIndex(selectedLocation);
            int hSelectedIndex = GetHIndex(selectedLocation);
            int wClickedIndex = GetWIndex(mouseClickLocation);
            int hClickedIndex = GetHIndex(mouseClickLocation);

            return info.IsRegionFromTargetArea(GameMode.Relocation, new Point(wSelectedIndex, hSelectedIndex), new Point(wClickedIndex, hClickedIndex));
        }

        public static RegionInformation GetSelectedRegion(this RegionInformation[,] info, Point selectedLocation)
        {
            int wSelectedIndex = GetWIndex(selectedLocation);
            if (wSelectedIndex < 0 || info.GetLength(0) <= wSelectedIndex)
                return null;

            int hSelectedIndex = GetHIndex(selectedLocation);
            if (hSelectedIndex < 0 || info.GetLength(1) <= hSelectedIndex)
                return null;

            return info[wSelectedIndex, hSelectedIndex];
        }

        // === Private methods section ===

        private static void TryAddRegion(this ICollection<RegionInformation> result, RegionInformation[,] info, int targetLandId, int wIndex, int hIndex)
        {
            RegionInformation i = info[wIndex, hIndex];
            if (i.LandId == targetLandId)
            {
                result.Add(i);
            }
        }

        private static bool IsRegionFromTargetArea(this RegionInformation[,] info, GameMode mode, Point targetCoordinates, Point coordinatesToCheck)
        {
            switch (mode)
            {
                case GameMode.Normal:
                {
                    return true;
                }
                case GameMode.Attack:
                {
                    return (Math.Abs(targetCoordinates.X - coordinatesToCheck.X) <= 1) &&
                           (Math.Abs(targetCoordinates.Y - coordinatesToCheck.Y) <= 1);
                }
                case GameMode.Relocation:
                {
                    int targetLandId = info[targetCoordinates.X, targetCoordinates.Y].LandId;
                    var usedList = new List<Point> {targetCoordinates};
                    for (int i = 0; i < usedList.Count; ++i)
                    {
                        int currentWIndex = usedList[i].X;
                        int currentHIndex = usedList[i].Y;

                        for (int wShift = -1; wShift <= 1; ++wShift)
                        {
                            for (int hShift = -1; hShift <= 1; ++hShift)
                            {
                                if (wShift == 0 && hShift == 0)
                                    continue;

                                if (info.GetLength(0) <= currentWIndex + wShift ||
                                    currentWIndex + wShift < 0 ||
                                    info.GetLength(1) <= currentHIndex + hShift ||
                                    currentHIndex + hShift < 0)
                                {
                                    continue;
                                }

                                var currentCoordinates = new Point(currentWIndex + wShift, currentHIndex + hShift);

                                if (targetLandId == info[currentCoordinates.X, currentCoordinates.Y].LandId &&
                                    !usedList.Contains(currentCoordinates))
                                {
                                    if (coordinatesToCheck.Equals(currentCoordinates))
                                        return true;
                                    usedList.Add(currentCoordinates);
                                }
                            }
                        }
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
