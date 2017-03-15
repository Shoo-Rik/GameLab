using Common.Data;
using GamePrototype.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace GamePrototype.Models
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

        public Model(int color)
        {
            _mapInfo = MapProcessor.InitializeMapInfo(color);
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

                if (info.Battles != null)
                {
                    foreach (var battle in info.Battles)
                    {
                        result.Add(string.Empty);
                        result.Add($"Атакован армией '{(battle.Attacker.LandId & 0xFFFFFF):X}' из региона ({battle.Attacker.From.X+1}:{battle.Attacker.From.Y+1})");
                        result.Add($"Численность: {battle.Attacker.Count}");
                    }
                }

                result.Add(string.Empty);
                result.Add("=== История сражений ===");
                foreach (Battle battle in _mapInfo.BattleHistory.Where(b => (b.From.X == wSelectedIndex) && (b.From.Y == hSelectedIndex)))
                {
                    result.Add(string.Empty);
                    result.Add($"ХОД {battle.Step}");
                    result.Add(string.Empty);
                    result.Add($"Атакующая армия '{(battle.Attacker.LandId & 0xFFFFFF):X}' из региона ({battle.Attacker.From.X + 1}:{battle.Attacker.From.Y + 1})");
                    result.Add($"Понесённый урон: {battle.AttackerDamage}");
                    result.Add($"Остаток армии: {battle.Attacker.Count}");
                    result.Add(string.Empty);
                    result.Add($"Защищающая армия '{(battle.Defender.LandId & 0xFFFFFF):X}'");
                    result.Add($"Понесённый урон: {battle.DefenderDamage}");
                    result.Add($"Остаток армии: {battle.Defender.Count}");
                    result.Add($"Остаток резерва: {battle.DefenderReserve.Count}");
                    result.Add("-------------------------------------------------");
                }
            }
            return result.ToArray();
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

        // [TODO]: Using constant 1
        public bool IsRegionAllowedToAttack(Point mouseClickLocation)
        {
            int attackedWIndex = MapProcessor.GetWIndex(mouseClickLocation);
            int attackedHIndex = MapProcessor.GetHIndex(mouseClickLocation);
            int currentWIndex = MapProcessor.GetWIndex(_selectedLocation);
            int currentHIndex = MapProcessor.GetHIndex(_selectedLocation);

            RegionInformation attackedRegion = GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(attackedWIndex - currentWIndex) <= 1 && Math.Abs(attackedHIndex - currentHIndex) <= 1)
                && attackedRegion != null && (attackedRegion.LandId != _mapInfo.OwnColor));
        }

        // [TODO]: Using constant 1
        public bool IsRegionAllowedToRelocation(Point mouseClickLocation)
        {
            int newWIndex = MapProcessor.GetWIndex(mouseClickLocation);
            int newHIndex = MapProcessor.GetHIndex(mouseClickLocation);
            int currentWIndex = MapProcessor.GetWIndex(_selectedLocation);
            int currentHIndex = MapProcessor.GetHIndex(_selectedLocation);

            // Skip the same region
            if (newWIndex == currentWIndex && newHIndex == currentHIndex)
                return false;

            RegionInformation newRegion = GetSelectedRegion(mouseClickLocation);

            return ((Math.Abs(newWIndex - currentWIndex) <= 1 && Math.Abs(newHIndex - currentHIndex) <= 1)
                && newRegion != null && (newRegion.LandId == _mapInfo.OwnColor));
        }

        public Bitmap GenerateMap()
        {
            return MapProcessor.GenerateMap(_mapInfo, _selectedLocation, _mode);
        }

        public void ProcessCurrentBattles()
        {
            for (int wIndex = 0; wIndex < _mapInfo.Info.GetLength(0); ++wIndex)
            {
                for (int hIndex = 0; hIndex < _mapInfo.Info.GetLength(1); ++hIndex)
                {
                    Battle[] battles = _mapInfo.Info[wIndex, hIndex].Battles;
                    if (battles == null)
                        continue;

                    foreach (Battle battle in battles)
                    {
                        if (battle.Step != _mapInfo.Step)
                        {
                            //throw new InvalidOperationException($"battle.Step ({battle.Step}) != _mapInfo.Step ({_mapInfo.Step})");
                            continue;
                        }

                        // Initialize Defender army
                        RegionInformation targetRegion = _mapInfo.Info[battle.Defender.From.X, battle.Defender.From.Y];
                        battle.Defender.Count = targetRegion.Army.Count;
                        battle.DefenderReserve.Count = targetRegion.Reserve.Count;

                        BattleProcessor.ProcessBattle(battle);

                        switch (battle.Result)
                        {
                            case BattleResult.AttackerWon:
                            {
                                // [TODO] Move defender army remainder to its near region
                                RegionInformation[] regions = MapProcessor.GetNearOwnRegions(_mapInfo, targetRegion.Coordinates, targetRegion.LandId);
                                if (regions != null && regions.Length > 0)
                                {
                                    regions[0].Army.Count += battle.Defender.Count;
                                }
                                else
                                {
                                    MessageBox.Show($"Игрок '{targetRegion.LandId}' проиграл!");
                                }

                                targetRegion.LandId = battle.Attacker.LandId;
                                targetRegion.Army.Count = battle.Attacker.Count;
                                targetRegion.Reserve.Count = 0; // [TODO]: Rule of calculation of Reserve count
                                break;
                            }
                            case BattleResult.AttackerLost:
                            {
                                targetRegion.Army.Count = battle.Defender.Count;
                                targetRegion.Reserve.Count = battle.DefenderReserve.Count;

                                RegionInformation attackerRegion = _mapInfo.Info[battle.Attacker.From.X, battle.Attacker.From.Y];
                                attackerRegion.Army.Count += battle.Attacker.Count;
                                break;
                            }
                        }
                    }

                    // Clear battles
                    _mapInfo.Info[wIndex, hIndex].Battles = null;
                }
            }
        }

        // === Private methods section ===

        private RegionInformation GetSelectedRegion(Point selectedLocation)
        {
            int wSelectedIndex = MapProcessor.GetWIndex(selectedLocation);
            if (wSelectedIndex <= 0 || _mapInfo.Info.GetLength(0) <= wSelectedIndex)
                return null;

            int hSelectedIndex = MapProcessor.GetHIndex(selectedLocation);
            if (hSelectedIndex <= 0 || _mapInfo.Info.GetLength(1) <= hSelectedIndex)
                return null;

            return _mapInfo.Info[wSelectedIndex, hSelectedIndex];
        }
    }
}
