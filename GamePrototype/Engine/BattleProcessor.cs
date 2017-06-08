using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Data;

namespace Engine
{
    public static class BattleProcessor
    {
        private static readonly int MaxDamagePercentDeviation = 30; // [TODO]
        private static readonly int MeanDamagePercent = 30; // [TODO]

        public static void SetBattleInRegion(int currentStep, RegionInformation fromRegion, RegionInformation toRegion, int armyCount,
            IList<Battle> allBattles)
        {
            // [TODO]: Join with other own regions?
            Battle battle = GetCurrentBattle(toRegion, fromRegion.Color, fromRegion.Coordinates, currentStep);
            int previousArmyCount = battle?.Attacker.Count ?? 0;

            //if (fromRegion.Army.Count < armyCount)
            //throw new InvalidOperationException("fromRegion.Army.Count < armyCount");

            fromRegion.Army.Count -= (armyCount - previousArmyCount);

            if (armyCount > 0)
            {
                if (battle == null)
                {
                    battle = new Battle
                    {
                        Attacker = new Army
                        {
                            Color = fromRegion.Color,
                            From = fromRegion.Coordinates,
                            Count = armyCount
                        },
                        Defender = new Army
                        {
                            Color = toRegion.Color,
                            From = toRegion.Coordinates,
                            // Count set dynamically
                        },
                        DefenderReserve = new Reserve
                        {
                            // Count set dynamically
                        },
                        Step = currentStep
                    };

                    Battle[] previousBattles = toRegion.Battles;
                    toRegion.Battles = previousBattles?.Concat(new[] { battle }).ToArray() ?? new[] { battle };

                    allBattles?.Add(battle);
                }
                else
                {
                    battle.Attacker.Count = armyCount;
                }
            }
            else if (armyCount == 0)
            {
                if (battle != null)
                {
                    toRegion.Battles = toRegion.Battles.Except(new[] { battle }).ToArray();
                }
                // [TODO]: Move to controller
                MessageBox.Show("Битва отменена.", "Отмена битвы", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static Battle GetCurrentBattle(RegionInformation defenderRegion, Color attackerColor, Coordinates attackerCoordinates, int currentStep)
        {
            Battle result = null;

            if (defenderRegion.Battles == null)
                return null;

            foreach (var battle in defenderRegion.Battles)
            {
                if ((battle.Attacker.Color == attackerColor) &&
                    (battle.Attacker.From.Equals(attackerCoordinates)) &&
                    (battle.Step == currentStep))
                {
                    result = battle;
                    break;
                }
            }

            return result;
        }

        public static void ProcessBattle(Battle battle)
        {
            // [TODO]
            int attackerCount = battle.Attacker.Count;
            int defenderCount = battle.Defender.Count + battle.DefenderReserve.Count;

            // Interval from -MaxDamagePercent to MaxDamagePercent
            int attackerPercentDeviation = MapProcessor.Rnd.Next(2 * MaxDamagePercentDeviation) - MaxDamagePercentDeviation;
            int defenderPercentDeviation = MapProcessor.Rnd.Next(2 * MaxDamagePercentDeviation) - MaxDamagePercentDeviation;

            double attackerDamagePercent = MeanDamagePercent * (1 + (double)attackerPercentDeviation / 100);
            double defenderDamagePercent = MeanDamagePercent * (1 + (double)defenderPercentDeviation / 100);

            battle.DamageToDefender = (int)(attackerCount * attackerDamagePercent / 100);
            battle.DamageToAttacker = (int)(defenderCount * defenderDamagePercent / 100);

            int newAttackerCount;
            int newDefenderCount;

            if (battle.DamageToAttacker < attackerCount && battle.DamageToDefender < defenderCount)
            {
                newAttackerCount = attackerCount - battle.DamageToAttacker;
                newDefenderCount = defenderCount - battle.DamageToDefender;

                battle.Result = (newAttackerCount > newDefenderCount)
                    ? BattleResult.AttackerWon
                    : BattleResult.AttackerLost; 
                // [TODO] BattleResult.Draw;
            }
            else if (battle.DamageToAttacker >= attackerCount && battle.DamageToDefender < defenderCount)
            {
                newAttackerCount = 0;
                newDefenderCount = defenderCount - battle.DamageToDefender * attackerCount / battle.DamageToAttacker;

                battle.Result = BattleResult.AttackerLost;
            }
            else if (battle.DamageToAttacker < attackerCount && battle.DamageToDefender >= defenderCount)
            {
                newAttackerCount = attackerCount - battle.DamageToAttacker * defenderCount / battle.DamageToDefender;
                newDefenderCount = 0;

                battle.Result = BattleResult.AttackerWon;
            }
            else // if (attackerDamage > defenderCount && defenderDamage > attackerCount)
            {
                throw new InvalidOperationException("Impossible case: attackerDamage > defenderCount && defenderDamage > attackerCount");
            }

            battle.Attacker.Count = newAttackerCount;
            battle.Defender.Count = newDefenderCount * battle.Defender.Count / defenderCount; // [TODO]: Proprtion must be changed?
            battle.DefenderReserve.Count = newDefenderCount - battle.Defender.Count;

            battle.DamageToAttacker = attackerCount - newAttackerCount;
            battle.DamageToDefender = defenderCount - newDefenderCount;
        }
    }
}
