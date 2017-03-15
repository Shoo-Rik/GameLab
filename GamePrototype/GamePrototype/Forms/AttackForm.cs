using Common.Data;
using GamePrototype.Models;
using System;
using System.Windows.Forms;

namespace GamePrototype.Forms
{
    public partial class AttackForm : Form
    {
        private readonly RegionInformation _sourceRegion;
        private readonly RegionInformation _attackedRegion;
        private readonly int _currentStep;

        public int ArmyCount
        {
            get { return (int)armyCountBox.Value; }
        }

        public AttackForm(int currentStep, RegionInformation sourceRegion, RegionInformation attackedRegion)
        {
            InitializeComponent();

            _currentStep = currentStep;
            _sourceRegion = sourceRegion;
            _attackedRegion = attackedRegion;
        }

        private void AttackForm_Load(object sender, EventArgs e)
        {
            Battle battle = BattleProcessor.GetCurrentBattle(_attackedRegion, _sourceRegion.LandId, _sourceRegion.Coordinates, _currentStep);
            int currentArmyCount = battle?.Attacker.Count ?? 0;

            int sourceArmyCount = _sourceRegion.Army.Count + currentArmyCount;

            trackBar1.Minimum = 0;
            trackBar1.Maximum = sourceArmyCount;

            armyCountBox.Minimum = 0;
            armyCountBox.Maximum = sourceArmyCount;

            // === Initialize ===

            trackBar1.Value = sourceArmyCount;
            armyCountBox.Value = sourceArmyCount;

            enemyArmyCountBox.Text = $"{_attackedRegion.Army.Count}";
            enemyReserveCountBox.Text = $"{_attackedRegion.Reserve.Count}";
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (armyCountBox.Value != ((TrackBar) sender).Value)
            {
                armyCountBox.Value = ((TrackBar) sender).Value;
            }
        }

        private void armyCountBox_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int) armyCountBox.Value;
        }

        private void armyCountBox_KeyUp(object sender, KeyEventArgs e)
        {
            decimal result;
            if (decimal.TryParse(armyCountBox.Text, out result))
            {
                if (result > armyCountBox.Maximum)
                    result = armyCountBox.Maximum;
                if (result < armyCountBox.Minimum)
                    result = armyCountBox.Minimum;

                armyCountBox.Value = result;
            }
        }
    }
}
