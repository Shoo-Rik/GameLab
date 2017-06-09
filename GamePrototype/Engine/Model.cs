using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Data;

namespace Engine
{
    public sealed class Model
    {
        // === Fields ===

        private readonly MapInfo _mapInfo;
        private Point _selectedLocation;
        private GameMode _mode = GameMode.Normal;

        public Func<RegionInformation, RegionInformation, int> GetArmyToAttackFunc { get; set; }
        public Func<RegionInformation, RegionInformation, bool> RelocateArmyFunc { get; set; }

        public delegate void ModeChanged(GameMode mode);
        public event ModeChanged ModeChangedEvent;

        public int Step
        {
            get { return _mapInfo.Step; }
        }

        // === Public methods section ===

        public Model(Color ownColor, Color enemy1Color, Color enemy2Color)
        {
            _mapInfo = MapProcessor.InitializeMapInfo(ownColor, enemy1Color, enemy2Color);
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

            int wSelectedIndex = MapProcessor.GetWIndex(selectedLocation);
            int hSelectedIndex = MapProcessor.GetHIndex(selectedLocation);

            if (wSelectedIndex > -1 && hSelectedIndex > -1)
            {
                RegionInformation info = _mapInfo.Info[wSelectedIndex, hSelectedIndex];
                result.Add(string.Empty);
                result.Add($"Армия: {info.Army.Count}");
                result.Add($"Резерв: {info.Reserve.Count}");
                result.Add($"Тотал: {info.Army.Count + info.Reserve.Count}");

                if (info.Battles != null)
                {
                    foreach (var battle in info.Battles)
                    {
                        string hRegionIndex = CoordinateHelper.HorizontalIndexToCoordinate(battle.Attacker.From.X);
                        string vRegionIndex = CoordinateHelper.VerticalIndexToCoordinate(battle.Attacker.From.Y);
                        string colorString = CoordinateHelper.GetColorName(battle.Attacker.Color);

                        result.Add(string.Empty);
                        result.Add($"Атакован армией '{colorString}' из региона {hRegionIndex}{vRegionIndex}");
                        result.Add($"Численность: {battle.Attacker.Count}");
                    }
                }

                result.Add(string.Empty);
                result.Add("=== История сражений ===");
                for (int i = _mapInfo.BattleHistory.Count - 1; i >= 0; --i)
                {
                    Battle battle = _mapInfo.BattleHistory[i];
                    if ((battle.From.X != wSelectedIndex) || (battle.From.Y != hSelectedIndex) || (battle.Step >= _mapInfo.Step))
                    {
                        continue;
                    }
                    string attackerResultString = battle.Result == BattleResult.AttackerWon ? "победила" : "проиграла";
                    string defenderResultString = battle.Result != BattleResult.AttackerWon ? "победила" : "проиграла";

                    string hRegionIndex = CoordinateHelper.HorizontalIndexToCoordinate(battle.Attacker.From.X);
                    string vRegionIndex = CoordinateHelper.VerticalIndexToCoordinate(battle.Attacker.From.Y);
                    string attackerColorString = CoordinateHelper.GetColorName(battle.Attacker.Color);
                    string defenderColorString = CoordinateHelper.GetColorName(battle.Defender.Color);

                    double attackerDamageRate = (double)(100 * battle.DamageToAttacker) / (battle.DamageToAttacker + battle.Attacker.Count);
                    double defenderDamageRate = (double)(100 * battle.DamageToDefender) / (battle.DamageToDefender + battle.DamageToDefender + battle.DefenderReserve.Count);

                    result.Add(string.Empty);
                    result.Add($"ХОД {battle.Step}");
                    result.Add(string.Empty);
                    result.Add($"Атакующая армия '{attackerColorString}' из региона {hRegionIndex}{vRegionIndex} {attackerResultString}");
                    result.Add($"Понесённый урон: {battle.DamageToAttacker} ({attackerDamageRate:F1}%)");
                    result.Add($"Остаток армии: {battle.Attacker.Count}");
                    result.Add(string.Empty);
                    result.Add($"Защищающая армия '{defenderColorString}' {defenderResultString}");
                    result.Add($"Понесённый урон: {battle.DamageToDefender} ({defenderDamageRate:F1}%)");
                    result.Add($"Остаток армии: {battle.Defender.Count}");
                    result.Add($"Остаток резерва: {battle.DefenderReserve.Count}");
                    result.Add("-------------------------------------------------");
                }
            }
            return result.ToArray();
        }

        public Color GetOwnColor()
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
                    currentRegion = _mapInfo.Info.GetSelectedRegion(_selectedLocation);
                    if (currentRegion == null || currentRegion.Color != _mapInfo.OwnColor)
                    {
                        return false;
                    }
                    SetMode(GameMode.Attack);
                    break;

                case GameAction.Relocation:
                    currentRegion = _mapInfo.Info.GetSelectedRegion(_selectedLocation);
                    if (currentRegion == null || currentRegion.Color != _mapInfo.OwnColor)
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

                    RegionInformation currentRegion = _mapInfo.Info.GetSelectedRegion(_selectedLocation);
                    RegionInformation attackedRegion = _mapInfo.Info.GetSelectedRegion(mouseClickLocation);

                    if (currentRegion == null || attackedRegion == null || GetArmyToAttackFunc == null)
                        break;

                    int armyCount = GetArmyToAttackFunc(currentRegion, attackedRegion);
                    if (armyCount >= 0)
                    {
                        BattleProcessor.SetBattleInRegion(_mapInfo.Step, currentRegion, attackedRegion, armyCount, _mapInfo.BattleHistory);
                        SetMode(GameMode.Normal);
                    }
                    break;
                }
                case GameMode.Relocation:
                {
                    if (!IsRegionAllowedToRelocation(mouseClickLocation))
                        break;

                    RegionInformation currentRegion = _mapInfo.Info.GetSelectedRegion(_selectedLocation);
                    RegionInformation newRegion = _mapInfo.Info.GetSelectedRegion(mouseClickLocation);

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

        private bool IsRegionAllowedToAttack(Point mouseClickLocation)
        {
            return _mapInfo.Info.IsRegionAllowedToAttack(mouseClickLocation, _selectedLocation, _mapInfo.OwnColor);
        }

        private bool IsRegionAllowedToRelocation(Point mouseClickLocation)
        {
            return _mapInfo.Info.IsRegionAllowedToRelocation(mouseClickLocation, _selectedLocation);
        }

        public Bitmap GenerateMap()
        {
            return _mapInfo.GenerateMap(_selectedLocation, _mode);
        }

        public Bitmap GenerateHorizontalHeader()
        {
            return _mapInfo.GenerateHorizontalHeader();
        }

        public Bitmap GenerateVerticalHeader()
        {
            return _mapInfo.GenerateVerticalHeader();
        }

        public void ProcessCurrentBattles()
        {
            RegionInformation[,] regions = _mapInfo.Info;

            for (int wIndex = 0; wIndex < regions.GetLength(0); ++wIndex)
            {
                for (int hIndex = 0; hIndex < regions.GetLength(1); ++hIndex)
                {
                    if (regions[wIndex, hIndex].Battles == null)
                        continue;

                    // Battles of current region for current step
                    Battle[] battles = regions[wIndex, hIndex].Battles.Where(b => b.Step == _mapInfo.Step).ToArray();
                    int battleCount = battles.Length;
                    if (battleCount == 0)
                        continue;

                    RegionInformation targetRegion = regions[wIndex, hIndex];

                    // TODO: Customize army distribution
                    int defArmyCount = targetRegion.Army.Count / battleCount;
                    int defArmyReminder = targetRegion.Army.Count % battleCount;
                    int defReserveCount = targetRegion.Reserve.Count / battleCount;
                    int defReserveReminder= targetRegion.Reserve.Count % battleCount;

                    for (int i = 0; i < battleCount; ++i)
                    {
                        Battle battle = battles[i];

                        // Initialize Defender army
                        battle.Defender.Count = (i == 0) ? (defArmyCount + defArmyReminder) : defArmyCount;
                        battle.DefenderReserve.Count = (i == 0) ? (defReserveCount + defReserveReminder) : defReserveCount;

                        BattleProcessor.ProcessBattle(battle);
                    }

                    // General result
                    IEnumerable<Battle> attackerWinners = battles.Where(b => b.Result == BattleResult.AttackerWon);
                    bool defenderLost = attackerWinners.Any();

                    RegionInformation regionToRetreat = null;
                    if (defenderLost)
                    {
                        RegionInformation[] ownRegions = regions.GetNearOwnRegions(targetRegion.Coordinates, targetRegion.Color);

                        if (ownRegions != null && ownRegions.Length > 0)
                        {
                            regionToRetreat = ownRegions[0];
                            //regionToRetreat.Army.Count += battle.Defender.Count;
                        }
                        else
                        {
                            MessageBox.Show($"Игрок '{targetRegion.Color}' проиграл!");
                        }
                    }

                    List<Battle> nextStepBattles = new List<Battle>();
                    Color winnerColor = Color.Empty; // TODO: Ensure this color cannot be a user color

                    // TODO: Process scenario with various winners
                    foreach (Battle battle in battles)
                    {
                        if (defenderLost)
                        {
                            switch (battle.Result)
                            {
                                case BattleResult.AttackerWon:
                                {
                                    if (winnerColor == Color.Empty)
                                    {
                                        winnerColor = targetRegion.Color = battle.Attacker.Color;
                                        targetRegion.Army.Count = battle.Attacker.Count;
                                        targetRegion.Reserve.Count = 0; // [TODO]: Rule of calculation of Reserve count
                                    }
                                    else if (winnerColor == battle.Attacker.Color)
                                    {
                                        targetRegion.Army.Count += battle.Attacker.Count;
                                    }
                                    else
                                    {
                                        nextStepBattles.Add(new Battle
                                        {
                                            Attacker = battle.Attacker,
                                            Defender = new Army
                                            {
                                                Color = winnerColor,
                                                From = targetRegion.Coordinates,
                                                // Count set dynamically
                                            },
                                            Step = _mapInfo.Step + 1
                                        });
                                    }

                                    if (regionToRetreat == null)
                                    {
                                        throw new InvalidOperationException("regionToRetreat == null");
                                    }
                                    regionToRetreat.Army.Count += battle.Defender.Count;
                                    break;
                                }
                                case BattleResult.AttackerLost:
                                {
                                    if (regionToRetreat == null)
                                    {
                                        throw new InvalidOperationException("regionToRetreat == null");
                                    }
                                    regionToRetreat.Army.Count += battle.Defender.Count;

                                    RegionInformation attackerRegion = regions[battle.Attacker.From.X, battle.Attacker.From.Y];
                                    attackerRegion.Army.Count += battle.Attacker.Count;
                                    break;
                                }
                                case BattleResult.Draw:
                                {
                                    // [TODO]
                                    break;
                                }
                            }
                        }
                        else // Defender did not lose
                        {
                            switch (battle.Result)
                            {
                                case BattleResult.AttackerWon:
                                {
                                    throw new InvalidOperationException("Defender did not lose, but an attacker won.");
                                    /*targetRegion.Color = battle.Attacker.Color;
                                    targetRegion.Army.Count = battle.Attacker.Count;
                                    targetRegion.Reserve.Count = 0; // [TODO]: Rule of calculation of Reserve count
                                    break;*/
                                }
                                case BattleResult.AttackerLost:
                                {
                                    targetRegion.Army.Count = battle.Defender.Count;
                                    targetRegion.Reserve.Count = battle.DefenderReserve.Count;

                                    RegionInformation attackerRegion = regions[battle.Attacker.From.X, battle.Attacker.From.Y];
                                    attackerRegion.Army.Count += battle.Attacker.Count;
                                    break;
                                }
                                case BattleResult.Draw:
                                {
                                    // [TODO]
                                    break;
                                }
                            }
                        }
                    }

                    // Replace battles
                    regions[wIndex, hIndex].Battles = nextStepBattles.ToArray();
                }
            }
        }
    }
}
