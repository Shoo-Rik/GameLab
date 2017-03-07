using Common.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GamePrototype.Models
{
    public sealed class Model
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

        // === Fields ===

        private static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        private readonly MapInfo _mapInfo;
        private Point _selectedLocation;
        private GameMode _mode = GameMode.Normal;

        public Func<RegionInformation, RegionInformation, int> GetArmyToAttackFunc { get; set; }
        public Func<RegionInformation, RegionInformation, bool> RelocateArmyFunc { get; set; }

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

                if (info.Battles != null)
                {
                    foreach (var battle in info.Battles)
                    {
                        result.Add(string.Empty);
                        result.Add($"Атакован армией '{(battle.Attacker.LandId & 0xFFFFFF):X}' из региона ({battle.Attacker.From.X+1}:{battle.Attacker.From.Y+1})");
                        result.Add($"Численность: {battle.Attacker.Count}");
                    }
                }
            }
            return result.ToArray();
        }

        // [TODO]: Refactor
        public Bitmap GenerateMap()
        {
            int widthCount = _mapInfo.Info.GetLength(0);
            int heightCount = _mapInfo.Info.GetLength(1);

            int wSelectedIndex = GetWIndex(_selectedLocation);
            int hSelectedIndex = GetHIndex(_selectedLocation);

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
                    else if (!((_mode == GameMode.Normal) ||
                               (_mode == GameMode.Attack || _mode == GameMode.Relocation) && (Math.Abs(hIndex - hSelectedIndex) <= 1) &&
                               (Math.Abs(wIndex - wSelectedIndex) <= 1)))
                    {
                        colorId = DisabledRegionColor;
                    }
                    else
                    {
                        colorId = _mapInfo.Info[wIndex, hIndex].LandId;
                        hasBattle = _mapInfo.Info[wIndex, hIndex].Battles != null &&
                                    _mapInfo.Info[wIndex, hIndex].Battles.Length > 0;
                    }

                    const int shift = 4;
                    int count = 0;

                    if (hasBattle)
                    {
                        foreach (var battle in _mapInfo.Info[wIndex, hIndex].Battles)
                        {
                            gr.FillRectangle(CreateBrush(battle.Attacker.LandId),
                                new Rectangle(
                                    wIndex*RegionSize + count*shift,
                                    hIndex*RegionSize + count*shift,
                                    RegionSize - 1 - 2*count*shift,
                                    RegionSize - 1 - 2*count*shift));
                            ++count;
                        }
                    }

                    gr.FillRectangle(CreateBrush(colorId),
                        new Rectangle(
                            wIndex*RegionSize + count*shift,
                            hIndex*RegionSize + count*shift,
                            RegionSize - 1 - 2*count*shift,
                            RegionSize - 1 - 2*count*shift));
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

        public bool InitiateAction(GameAction action)
        {
            RegionInformation currentRegion;
            switch (action)
            {
                case GameAction.Attack:
                    currentRegion = GetSelectedRegion(_selectedLocation);
                    if (currentRegion == null || currentRegion.LandId != _mapInfo.OwnColor)
                    {
                        return false;
                    }
                    SetMode(GameMode.Attack);
                    break;

                case GameAction.Relocation:
                    currentRegion = GetSelectedRegion(_selectedLocation);
                    if (currentRegion == null || currentRegion.LandId != _mapInfo.OwnColor)
                    {
                        return false;
                    }
                    SetMode(GameMode.Relocation);
                    break;

                case GameAction.Cancel:
                    SetMode(GameMode.Normal);
                    break;
            }
            return true;
        }

        // [TODO]: Refactor: Replace Point to RegionInformation coordinates
        public void OnChooseLocation(Point mouseClickLocation)
        {
            switch (_mode)
            {
                case GameMode.Normal:
                {
                    _selectedLocation = mouseClickLocation;
                    break;
                }
                case GameMode.Attack:
                {
                    if (!IsRegionAllowedToAttack(mouseClickLocation))
                        break;

                    RegionInformation currentRegion = GetSelectedRegion(_selectedLocation);
                    RegionInformation attackedRegion = GetSelectedRegion(mouseClickLocation);

                    if (currentRegion == null || attackedRegion == null || GetArmyToAttackFunc == null)
                        break;

                    int armyCount = GetArmyToAttackFunc(currentRegion, attackedRegion);
                    if (armyCount > 0)
                    {
                        SetBattleInRegion(_mapInfo.Step, currentRegion, attackedRegion, armyCount);
                        SetMode(GameMode.Normal);
                    }
                    break;
                }
                case GameMode.Relocation:
                {
                    if (!IsRegionAllowedToRelocation(mouseClickLocation))
                        break;

                    RegionInformation currentRegion = GetSelectedRegion(_selectedLocation);
                    RegionInformation newRegion = GetSelectedRegion(mouseClickLocation);

                    if (currentRegion == null || newRegion == null || RelocateArmyFunc == null)
                        break;

                    if (RelocateArmyFunc(currentRegion, newRegion))
                    {
                        SetMode(GameMode.Normal);
                    }
                    break;
                }
            }
        }

        private static void SetBattleInRegion(int currentStep, RegionInformation fromRegion, RegionInformation toRegion, int armyCount)
        {
            if (fromRegion.Army.Count < armyCount)
                throw new InvalidOperationException("fromRegion.Army.Count < armyCount");

            fromRegion.Army.Count -= armyCount;

            // [TODO]
            var battle = new Battle
            {
                Attacker = new Army
                {
                    LandId = fromRegion.LandId,
                    From = fromRegion.Coordinates,
                    Count = armyCount
                },
                Defender = new Army
                {
                    LandId = toRegion.LandId,
                    From = toRegion.Coordinates
                },
                Step = currentStep
            };

            Battle[] previousBattles = toRegion.Battles;
            toRegion.Battles = (previousBattles == null) 
                ? new[] { battle } 
                : previousBattles.Concat(new[] { battle }).ToArray();
        }

        // [TODO]: Using constant 1
        public bool IsRegionAllowedToAttack(Point mouseClickLocation)
        {
            int attackedWIndex = GetWIndex(mouseClickLocation);
            int attackedHIndex = GetHIndex(mouseClickLocation);
            int currentWIndex = GetWIndex(_selectedLocation);
            int currentHIndex = GetHIndex(_selectedLocation);

            RegionInformation attackedRegion = GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1)
                && attackedRegion != null && (attackedRegion.LandId != _mapInfo.OwnColor));
        }

        // [TODO]: Using constant 1
        public bool IsRegionAllowedToRelocation(Point mouseClickLocation)
        {
            int newWIndex = GetWIndex(mouseClickLocation);
            int newHIndex = GetHIndex(mouseClickLocation);
            int currentWIndex = GetWIndex(_selectedLocation);
            int currentHIndex = GetHIndex(_selectedLocation);

            RegionInformation newRegion = GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(newWIndex - currentWIndex) <= 1 && Math.Abs(newHIndex - currentHIndex) <= 1)
                && newRegion != null && (newRegion.LandId == _mapInfo.OwnColor));
        }

        public static SolidBrush CreateBrush(int colorId)
        {
           return new SolidBrush(Color.FromArgb(colorId));
        }

        // === Private methods section ===

        private static MapInfo InitializeMapInfo(int color)
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

                        default: throw new NotSupportedException("Random value");
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

        private static int GetWIndex(Point location)
        {
            return location.X == 0 ? -1 : location.X / RegionSize;
        }

        private static int GetHIndex(Point location)
        {
            return location.Y == 0 ? -1 : location.Y / RegionSize;
        }

        private RegionInformation GetSelectedRegion(Point selectedLocation)
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
