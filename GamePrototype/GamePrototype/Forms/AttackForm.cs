﻿using System;
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
            get { return int.Parse(armyCountBox.Text); }
        }

        public AttackForm(RegionInformation sourceRegion, RegionInformation attackedRegion)
        {
            InitializeComponent();

            _sourceRegion = sourceRegion;
            _attackedRegion = attackedRegion;
        }

        private void AttackForm_Load(object sender, EventArgs e)
        {
            armyCountBox.Text = $"{_sourceRegion.Army.Count}";
            enemyArmyCountBox.Text = $"{_attackedRegion.Army.Count}";
            enemyReserveCountBox.Text = $"{_attackedRegion.Reserve.Count}";
            trackBar1.Minimum = 0;
            trackBar1.Maximum = _sourceRegion.Army.Count;
            trackBar1.Value = _sourceRegion.Army.Count;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            armyCountBox.Text = $"{((TrackBar) sender).Value}";
        }
    }
}