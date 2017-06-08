using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Data;

namespace Engine
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
        private static int RegionBorderSize = 4;

        #endregion [TODO]: Move to config

        internal static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        public static MapInfo InitializeMapInfo(Color ownColor, Color enemy1Color, Color enemy2Color)
        {
            var result = new MapInfo { OwnColor = ownColor };

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
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = ownColor };
                            break;

                        case 1:
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = enemy1Color };
                            break;

                        case 2:
                            result.Info[wIndex, hIndex] = new RegionInformation { Color = enemy2Color };
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

                    DrawRegion(gr, mapInfo.Info, hasBattle, wIndex, hIndex, color);
                }
            }
            return bmp;
        }

        private static void DrawRegion(Graphics gr, RegionInformation[,] info, bool hasBattles, int wIndex, int hIndex, Color color)
        {
            if (wIndex < 0 || hIndex < 0)
                return;

            int count = 1;
            //int enemyCount = 0;

            Battle[] battles = hasBattles ? info[wIndex, hIndex].Battles : null;

            // Mark attacked region
            if (battles != null)
            {
                foreach (var battle in battles)
                {
                    Direction attackerDirection = battle.Defender.From.GetAttackerDirection(battle.Attacker.From);

                    Rectangle battleRectangle;

                    switch (attackerDirection)
                    {
                        case Direction.Nord:
                            battleRectangle = new Rectangle(
                                wIndex * RegionSize,
                                hIndex * RegionSize,
                                RegionSize,
                                RegionBorderSize - 1);
                            break;

                        case Direction.Ost:
                            battleRectangle = new Rectangle(
                                (wIndex + 1) * RegionSize - RegionBorderSize + 1,
                                hIndex * RegionSize,
                                RegionBorderSize - 1,
                                RegionSize);
                            break;

                        case Direction.South:
                            battleRectangle = new Rectangle(
                                wIndex * RegionSize,
                                (hIndex + 1) * RegionSize - RegionBorderSize + 1,
                                RegionSize,
                                RegionBorderSize - 1);
                            break;

                        case Direction.West:
                            battleRectangle = new Rectangle(
                                wIndex * RegionSize,
                                hIndex * RegionSize,
                                RegionBorderSize - 1,
                                RegionSize);
                            break;

                        default:
                            continue;
                    }
                   
                    gr.FillRectangle(CreateBrush(battle.Attacker.Color), battleRectangle);
                    //++count;
                    //enemyCount += battle.Attacker.Count;
                }
            }

            gr.FillRectangle(CreateBrush(color),
                new Rectangle(
                    wIndex * RegionSize + count * RegionBorderSize,
                    hIndex * RegionSize + count * RegionBorderSize,
                    RegionSize - 2 * count * RegionBorderSize,
                    RegionSize - 2 * count * RegionBorderSize));

            // Display region info
            int a = info[wIndex, hIndex].Army.Count;
            int r = info[wIndex, hIndex].Reserve.Count;
            //int t = a + r;
            var printedInfo = new StringBuilder();
            printedInfo.AppendLine($"{a} / {r}");
            /*if (enemyCount > 0)
            {
                printedInfo.AppendLine($"------------");
                printedInfo.AppendLine($"E: {enemyCount}");
            }*/

            //const int textShift = 4;

            var rectangle = new Rectangle(
                wIndex * RegionSize + RegionBorderSize,
                hIndex * RegionSize + RegionBorderSize,
                RegionSize - 2 * RegionBorderSize,
                RegionSize - 2 * RegionBorderSize);

            Font font = new Font(FontFamily.GenericSansSerif, 8);
            Brush brush = CreateBrush(Color.Black);
            gr.DrawString(printedInfo.ToString(), font, brush, rectangle);
        }

        public static Bitmap GenerateHorizontalHeader(this MapInfo mapInfo)
        {
            int widthCount = mapInfo.Info.GetLength(0);
            int heightCount = 1; //mapInfo.Info.GetLength(1);

            Bitmap bmp = new Bitmap(RegionSize*widthCount, RegionSize*heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            //const int shift = 4;
            int hIndex = 0;
            for (int wIndex = 0; wIndex < widthCount; ++wIndex)
            {
                var rectangle = new Rectangle(
                    wIndex*RegionSize + RegionBorderSize,
                    hIndex*RegionSize,
                    RegionSize - 1 - 2 * RegionBorderSize,
                    RegionSize / 2);

                gr.FillRectangle(CreateBrush(Color.Coral), rectangle);

                Font font = new Font(FontFamily.GenericSansSerif, 16);
                Brush brush = CreateBrush(Color.Black);
                string printedInfo = new string((char)('А' + ((wIndex == 9) ? wIndex+1 : wIndex)), 1);
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

            //const int shift = 4;
            int wIndex = 0;
            for (int hIndex = 0; hIndex < heightCount; ++hIndex)
            {
                var rectangle = new Rectangle(wIndex * RegionSize, hIndex * RegionSize + RegionBorderSize, RegionSize / 2, RegionSize - 1 - 2 * RegionBorderSize);

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
            int relativeCoordinate = location.X % RegionSize;

            return (relativeCoordinate < RegionBorderSize || relativeCoordinate > RegionSize - RegionBorderSize) ? -1 : location.X / RegionSize;
        }

        public static int GetHIndex(Point location)
        {
            int relativeCoordinate = location.Y % RegionSize;

            return (relativeCoordinate < RegionBorderSize || relativeCoordinate > RegionSize - RegionBorderSize) ? -1 : location.Y / RegionSize;
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

            //bool isNearRegion = Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1;
            bool isNearRegion = (Math.Abs(attackedWIndex - currentWIndex) + Math.Abs(attackedHIndex - currentHIndex) == 1);

            return isNearRegion && (attackedRegion != null) && (attackedRegion.Color != ownColor);
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
                    //return (Math.Abs(targetCoordinates.X - coordinatesToCheck.X) <= 1) && (Math.Abs(targetCoordinates.Y - coordinatesToCheck.Y) <= 1);
                    return (Math.Abs(targetCoordinates.X - coordinatesToCheck.X) + Math.Abs(targetCoordinates.Y - coordinatesToCheck.Y) == 1);
                }
                case GameMode.Relocation: // [TODO]
                {
                    Color targetColor = info[targetCoordinates.X, targetCoordinates.Y].Color;
                    var usedList = new List<Point> { targetCoordinates };

                    for (int i = 0; i < usedList.Count; ++i)
                    {
                        int currentWIndex = usedList[i].X;
                        int currentHIndex = usedList[i].Y;

                        var arr = new []
                        {
                            new Point(currentWIndex - 1, currentHIndex),
                            new Point(currentWIndex + 1, currentHIndex),
                            new Point(currentWIndex, currentHIndex - 1),
                            new Point(currentWIndex, currentHIndex + 1)
                        };

                        foreach (var currentCoordinates in arr)
                        {
                            // Check boundaries
                            if (info.GetLength(0) <= currentCoordinates.X || currentCoordinates.X < 0 ||
                                info.GetLength(1) <= currentCoordinates.Y || currentCoordinates.Y < 0)
                            {
                                continue;
                            }

                            if (targetColor == info[currentCoordinates.X, currentCoordinates.Y].Color &&
                                !usedList.Contains(currentCoordinates))
                            {
                                if (coordinatesToCheck.Equals(currentCoordinates))
                                    return true;
                                usedList.Add(currentCoordinates);
                            }
                        }

                        /*for (int wShift = -1; wShift <= 1; ++wShift)
                        {
                            for (int hShift = -1; hShift <= 1; ++hShift)
                            {
                                if (wShift == 0 && hShift == 0)
                                    continue;

                                if (info.GetLength(0) <= currentWIndex + wShift || currentWIndex + wShift < 0 ||
                                    info.GetLength(1) <= currentHIndex + hShift || currentHIndex + hShift < 0)
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
                        }*/
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
