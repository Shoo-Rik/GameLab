using Common.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Win32;

namespace GamePrototype
{
    public sealed class Model
    {
        #region [TODO]: Move to config

        private static readonly int DefaultArmyCount = 1000;
        private static readonly int ArmyPercentVariation = 25;

        private static readonly int DefaultReserveCount = 2000;
        private static readonly int ReservePercentVariation = 35;

        private static readonly int SelectedRegionColor = 0xAAAAAA;
        private static readonly int DisabledRegionColor = 0x555555;

        private static int RegionSize = 64;

        #endregion [TODO]: Move to config

        // === Fields ===

        private static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        private readonly MapInfo _mapInfo;
        private Point _selectedLocation;
        private GameMode _mode = GameMode.Normal;

        public Func<RegionInformation, RegionInformation, int> GetArmyToAttackFunc { get; set; }

        public delegate void ModeChanged(GameMode mode);
        public event ModeChanged ModeChangedEvent;

        // === Public methods section ===

        public Model(int color)
        {
            _mapInfo = InitializeMapInfo(color);
        }

        public Model(string filePath)
        {
            _mapInfo = MapInfoSerializer.Deserialize(filePath);
        }

        public void Save(string filePath)
        {
            MapInfoSerializer.Serialize(_mapInfo, filePath);
        }
        
        public string[] GetRegionInfoStrings(Point? location = null)
        {
            var result = new List<string>();

            Point selectedLocation = location ?? _selectedLocation;

            // Show step
            result.Add($"Ход {_mapInfo.Step}");

            int wSelectedIndex = GetWIndex(selectedLocation);
            int hSelectedIndex = GetHIndex(selectedLocation);

            if (wSelectedIndex > -1 && hSelectedIndex > -1)
            {
                RegionInformation info = _mapInfo.Info[wSelectedIndex, hSelectedIndex];
                result.Add(string.Empty);
                result.Add($"Армия: {info.Army.Count}");
                result.Add($"Резерв: {info.Reserve.Count}");
            }
            return result.ToArray();
        }

        public Bitmap GenerateMap()
        {
            int widthCount = _mapInfo.Info.GetLength(0);
            int heightCount = _mapInfo.Info.GetLength(1);

            int wSelectedIndex = GetWIndex(_selectedLocation);
            int hSelectedIndex = GetHIndex(_selectedLocation);

            Bitmap bmp = new Bitmap(RegionSize * widthCount, RegionSize * heightCount);
            Graphics gr = Graphics.FromImage(bmp);

            for (int widthIndex = 0; widthIndex < widthCount; ++widthIndex)
            {
                for (int heightIndex = 0; heightIndex < heightCount; ++heightIndex)
                {
                    int colorId = _mapInfo.Info[widthIndex, heightIndex].LandId;
                    if (wSelectedIndex == widthIndex && hSelectedIndex == heightIndex)
                    {
                        colorId = SelectedRegionColor;
                    }

                    bool hasNormalColor = (_mode == GameMode.Normal) || (_mode == GameMode.Attack) &&
                        (Math.Abs(heightIndex - hSelectedIndex) <= 1) && (Math.Abs(widthIndex - wSelectedIndex) <= 1);

                    gr.FillRectangle(CreateBrush(hasNormalColor ? colorId : DisabledRegionColor),
                        new Rectangle(widthIndex * RegionSize, heightIndex * RegionSize - 1, RegionSize - 1, RegionSize - 1));
                }
            }
            return bmp;
        }

        public int GetOwnColor()
        {
            return _mapInfo.OwnColor;
        }

        public void IncrementStep()
        {
            _mapInfo.Step++;
        }

        private void SetMode(GameMode mode)
        {
            _mode = mode;

            ModeChangedEvent?.Invoke(mode);
        }

        public bool ProcessAction(GameAction action)
        {
            switch (action)
            {
                case GameAction.Attack:
                    RegionInformation? currentRegion = GetSelectedRegion(_selectedLocation);
                    if (!currentRegion.HasValue || currentRegion.Value.LandId != _mapInfo.OwnColor)
                    {
                        return false;
                    }
                    SetMode(GameMode.Attack);
                    break;

                case GameAction.Cancel:
                    SetMode(GameMode.Normal);
                    break;
            }
            return true;
        }

        public void ProcessMouseClick(Point mouseClickLocation)
        {
            switch (_mode)
            {
                case GameMode.Normal:
                    _selectedLocation = mouseClickLocation;
                    break;

                case GameMode.Attack:
                    if (!IsRegionAllowedToAttack(mouseClickLocation))
                        break;

                    RegionInformation? currentRegion = GetSelectedRegion(_selectedLocation);
                    RegionInformation? attackedRegion = GetSelectedRegion(mouseClickLocation);

                    if (!currentRegion.HasValue || !attackedRegion.HasValue || GetArmyToAttackFunc == null)
                        break;

                    int armyCount = GetArmyToAttackFunc(currentRegion.Value, attackedRegion.Value);
                    if (armyCount <= 0)
                        break;

                    // [TODO]

                    SetMode(GameMode.Normal);

                    break;
            }
        }

        // [TODO]: Using constant 1
        public bool IsRegionAllowedToAttack(Point mouseClickLocation)
        {
            int attackedWIndex = GetWIndex(mouseClickLocation);
            int attackedHIndex = GetHIndex(mouseClickLocation);
            int currentWIndex = GetWIndex(_selectedLocation);
            int currentHIndex = GetHIndex(_selectedLocation);

            RegionInformation? attackedRegion = GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1)
                && attackedRegion.HasValue && (attackedRegion.Value.LandId != _mapInfo.OwnColor));
        }

        public static SolidBrush CreateBrush(int colorId)
        {
            int red = (colorId & 0xFF0000) >> 16;
            int green = (colorId & 0x00FF00) >> 8;
            int blue = colorId & 0x0000FF;
            return new SolidBrush(Color.FromArgb(red, green, blue));
        }

        // === Private methods section ===

        private static MapInfo InitializeMapInfo(int color)
        {
            var result = new MapInfo { OwnColor = color };

            var info1 = new RegionInformation { LandId = 0xFF0000 };
            var info2 = new RegionInformation { LandId = 0x00FF00 };
            var info3 = new RegionInformation { LandId = 0x0000FF };

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
                            result.Info[wIndex, hIndex] = info1;
                            break;

                        case 1:
                            result.Info[wIndex, hIndex] = info2;
                            break;

                        case 2:
                            result.Info[wIndex, hIndex] = info3;
                            break;

                        default: throw new NotSupportedException("Random value");
                    }

                    int armyCount = DefaultArmyCount * (100 - ArmyPercentVariation + Rnd.Next(2 * ArmyPercentVariation)) / 100;
                    result.Info[wIndex, hIndex].Army = new Army { Count = armyCount };

                    int reserveCount = DefaultReserveCount * (100 - ReservePercentVariation + Rnd.Next(2 * ReservePercentVariation)) / 100;
                    result.Info[wIndex, hIndex].Reserve = new Reserve { Count = reserveCount };
                }
            }
            return result;
        }

        private static int GetWIndex(Point location)
        {
            return location.X == 0 ? -1 : location.X / RegionSize;
        }

        private static int GetHIndex(Point location)
        {
            return location.Y == 0 ? -1 : location.Y / RegionSize;
        }

        private RegionInformation? GetSelectedRegion(Point selectedLocation)
        {
            int wSelectedIndex = GetWIndex(selectedLocation);
            if (wSelectedIndex <= 0 || _mapInfo.Info.GetLength(0) <= wSelectedIndex)
                return null;

            int hSelectedIndex = GetHIndex(selectedLocation);
            if (hSelectedIndex <= 0 || _mapInfo.Info.GetLength(1) <= hSelectedIndex)
                return null;

            return _mapInfo.Info[wSelectedIndex, hSelectedIndex];
        }
    }
}
