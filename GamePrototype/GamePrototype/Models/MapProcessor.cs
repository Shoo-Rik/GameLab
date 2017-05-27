using Common.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GamePrototype.Models
{
    public static class MapProcessor
    {
        #region [TODO]: Move to config

        private static readonly int DefaultArmyCount = 1000;
        private static readonly int ArmyPercentVariation = 25;

        private static readonly int DefaultReserveCount = 2000;
        private static readonly int ReservePercentVariation = 35;

        private static readonly Color SelectedRegionColor = Color.DarkSlateGray;
        private static readonly Color DisabledRegionColor = Color.Gray;

        private static int RegionSize = 64;

        #endregion [TODO]: Move to config

        internal static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        public static MapInfo InitializeMapInfo(Color color)
        {
            var result = new MapInfo { OwnColor = color };

            var color1 = Color.Red;
            var color2 = Color.Green;
            var color3 = Color.Blue;

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
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = color1 };
                            break;

                        case 1:
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = color2 };
                            break;

                        case 2:
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = color3 };
                            break;

                        default:
                            throw new NotSupportedException("Random value");
                    }

                    result.Info[wIndex, hIndex].Coordinates = new Coordinates { X = wIndex, Y = hIndex };

                    int armyCount = DefaultArmyCount * (100 - ArmyPercentVariation + Rnd.Next(2 * ArmyPercentVariation)) / 100;
                    result.Info[wIndex, hIndex].Army = new Army
                    {
                        Count = armyCount,
                        Color = result.Info[wIndex, hIndex].Color
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
                    Color color;
                    if (wSelectedIndex == wIndex && hSelectedIndex == hIndex)
                    {
                        color = SelectedRegionColor;
                    }
                    else if (!mapInfo.Info.IsRegionFromTargetArea(mode, new Point(wSelectedIndex, hSelectedIndex), new Point(wIndex, hIndex)))
                    {
                        color = DisabledRegionColor;
                    }
                    else
                    {
                        color = mapInfo.Info[wIndex, hIndex].Color;
                        hasBattle = mapInfo.Info[wIndex, hIndex].Battles != null &&
                                    mapInfo.Info[wIndex, hIndex].Battles.Length > 0;
                    }

                    const int shift = 4;
                    int count = 0;
                    int enemyCount = 0;

                    // Mark attacked region
                    if (hasBattle)
                    {
                        foreach (var battle in mapInfo.Info[wIndex, hIndex].Battles)
                        {
                            gr.FillRectangle(CreateBrush(battle.Attacker.Color),
                                new Rectangle(
                                    wIndex * RegionSize + count * shift,
                                    hIndex * RegionSize + count * shift,
                                    RegionSize - 1 - 2 * count * shift,
                                    RegionSize - 1 - 2 * count * shift));
                            ++count;
                            enemyCount += battle.Attacker.Count;
                        }
                    }

                    gr.FillRectangle(CreateBrush(color),
                        new Rectangle(
                            wIndex * RegionSize + count * shift,
                            hIndex * RegionSize + count * shift,
                            RegionSize - 1 - 2 * count * shift,
                            RegionSize - 1 - 2 * count * shift));

                    // Display region info
                    int a = mapInfo.Info[wIndex, hIndex].Army.Count;
                    int r = mapInfo.Info[wIndex, hIndex].Reserve.Count;
                    int t = a + r;
                    var printedInfo = new StringBuilder();
                    printedInfo.AppendLine($"A: {a}");
                    printedInfo.AppendLine($"R: {r}");
                    printedInfo.AppendLine($"T: {t}");
                    if (enemyCount > 0)
                    {
                        printedInfo.AppendLine($"------------");
                        printedInfo.AppendLine($"E: {enemyCount}");
                    }

                    var rectangle = new Rectangle(
                        wIndex * RegionSize,
                        hIndex * RegionSize,
                        RegionSize - 1,
                        RegionSize - 1);

                    Font font = new Font(FontFamily.GenericSansSerif, 8);
                    Brush brush = CreateBrush(Color.Black);
                    gr.DrawString(printedInfo.ToString(), font, brush, rectangle);
                }
            }
            return bmp;
        }

        public static Bitmap GenerateHorizontalHeader(this MapInfo mapInfo)
        {
            int widthCount = mapInfo.Info.GetLength(0);
            int heightCount = 1; //mapInfo.Info.GetLength(1);

            Bitmap bmp = new Bitmap(RegionSize*widthCount, RegionSize*heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            int hIndex = 0;
            for (int wIndex = 0; wIndex < widthCount; ++wIndex)
            {
                var rectangle = new Rectangle(
                    wIndex*RegionSize,
                    hIndex*RegionSize,
                    RegionSize - 1,
                    RegionSize / 2);

                gr.FillRectangle(CreateBrush(Color.Coral), rectangle);

                Font font = new Font(FontFamily.GenericSansSerif, 16);
                Brush brush = CreateBrush(Color.Black);
                string printedInfo = new string((char)((char)'А' + ((wIndex == 9) ? wIndex+1 : wIndex)), 1);
                gr.DrawString(printedInfo, font, brush, rectangle);
            }
            return bmp;
        }

        public static Bitmap GenerateVerticalHeader(this MapInfo mapInfo)
        {
            int widthCount = 1;
            int heightCount = mapInfo.Info.GetLength(1);

            Bitmap bmp = new Bitmap(RegionSize * widthCount, RegionSize * heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            int wIndex = 0;
            for (int hIndex = 0; hIndex < heightCount; ++hIndex)
            {
                var rectangle = new Rectangle(
                    wIndex * RegionSize,
                    hIndex * RegionSize,
                    RegionSize / 2,
                    RegionSize - 1);

                gr.FillRectangle(CreateBrush(Color.DeepSkyBlue), rectangle);

                Font font = new Font(FontFamily.GenericSansSerif, 16);
                Brush brush = CreateBrush(Color.Black);
                string printedInfo = (hIndex + 1).ToString();
                gr.DrawString(printedInfo, font, brush, rectangle);
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

        public static SolidBrush CreateBrush(Color color)
        {
            return new SolidBrush(color);
        }

        public static RegionInformation[] GetNearOwnRegions(this RegionInformation[,] info, Coordinates coordinates, Color color)
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
                        result.TryAddRegion(info, color, wIndex, hIndex);
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
                        result.TryAddRegion(info, color, wIndex, hIndex);
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
                        result.TryAddRegion(info, color, wIndex, hIndex);
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
                        result.TryAddRegion(info, color, wIndex, hIndex);
                    }
                }
            }

            return result.ToArray();
        }

        // [TODO]: Using constant 1
        public static bool IsRegionAllowedToAttack(this RegionInformation[,] info, Point mouseClickLocation, Point selectedLocation, Color ownColor)
        {
            int attackedWIndex = GetWIndex(mouseClickLocation);
            int attackedHIndex = GetHIndex(mouseClickLocation);
            int currentWIndex = GetWIndex(selectedLocation);
            int currentHIndex = GetHIndex(selectedLocation);

            RegionInformation attackedRegion = info.GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1)
                && attackedRegion != null && (attackedRegion.Color != ownColor));
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

        private static void TryAddRegion(this ICollection<RegionInformation> result, RegionInformation[,] info, Color targetColor, int wIndex, int hIndex)
        {
            RegionInformation i = info[wIndex, hIndex];
            if (i.Color == targetColor)
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
                    Color targetColor = info[targetCoordinates.X, targetCoordinates.Y].Color;
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

                                if (targetColor == info[currentCoordinates.X, currentCoordinates.Y].Color &&
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
