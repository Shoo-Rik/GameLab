using System;
using System.Windows.Forms;
using Common.Data;

namespace GamePrototype.Forms
{
    public partial class AttackForm : Form
    {
        private readonly RegionInformation _sourceRegion;
        private readonly RegionInformation _attackedRegion;

        public int ArmyCount
        {
            get { return (int)armyCountBox.Value; }
        }

        public AttackForm(RegionInformation sourceRegion, RegionInformation attackedRegion)
        {
            InitializeComponent();

            _sourceRegion = sourceRegion;
            _attackedRegion = attackedRegion;
        }

        private void AttackForm_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = _sourceRegion.Army.Count;

            armyCountBox.Minimum = 0;
            armyCountBox.Maximum = _sourceRegion.Army.Count;

            // === Initialize ===

            trackBar1.Value = _sourceRegion.Army.Count;
            armyCountBox.Value = _sourceRegion.Army.Count;

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
