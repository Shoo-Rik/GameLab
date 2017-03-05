using Common.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GamePrototype
{
    public partial class MapForm : Form
    {
        private readonly Model _model;

        public MapForm(Model model)
        {
            InitializeComponent();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _model.ModeChangedEvent += OnModeChanged;
        }

        private void OnModeChanged(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Attack:
                    EnableActionButtons(false);
                    break;

                case GameMode.Normal:
                    EnableActionButtons(true);
                    break;
            }
        }

        private void EnableActionButtons(bool enable)
        {
            btnDefendRegion.Enabled = enable;
            btnGetReserve.Enabled = enable;
            btnJoinArmies.Enabled = enable;
            btnMakeStep.Enabled = enable;
            btnMoveArmy.Enabled = enable;
            btnSplitArmy.Enabled = enable;
            btnAttackNearRegion.Enabled = enable;
        }

        private void UpdateListBox(string[] strings)
        {
            listBox1.Items.Clear();
            Array.ForEach(strings, item => listBox1.Items.Add(item));
        }

        private void mapBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_model.GenerateMap(), new Point(0, 0));
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            _model.GetArmyToAttackFunc = GetArmyCountToAttack;

            UpdateListBox(_model.GetRegionInfoStrings());
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (var saveDlg = new SaveFileDialog())
            {
                if (saveDlg.ShowDialog() != DialogResult.OK)
                    return;

                _model.Save(saveDlg.FileName);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ownColorBox_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.FillRectangle(Model.CreateBrush(_model.GetOwnColor()), e.ClipRectangle);
            e.Graphics.DrawImage(bmp, new Point(0, 0));
        }

        private void mapBox_MouseClick(object sender, MouseEventArgs e)
        {
            _model.ProcessMouseClick(e.Location);
            UpdateListBox(_model.GetRegionInfoStrings());
            Refresh();
        }

        private void btnAttackNearRegion_Click(object sender, EventArgs e)
        {
            if (!_model.ProcessAction(GameAction.Attack))
            {
                MessageBox.Show(this, "Сначала выберите свой регион.", "Атаковать соседний регион", MessageBoxButtons.OK);
                return;
            }

            Refresh();

            MessageBox.Show("Выберите регион для атаки.", "Атака", MessageBoxButtons.OK);
        }

        private void btnDefendRegion_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnMoveArmy_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnJoinArmies_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnSplitArmy_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnGetReserve_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnMakeStep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Вы уверены?", "Сделать ход", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            _model.IncrementStep();

            UpdateListBox(_model.GetRegionInfoStrings());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _model.ProcessAction(GameAction.Cancel);

            Refresh();
        }

        private int GetArmyCountToAttack(RegionInformation sourceRegion, RegionInformation attackedRegion)
        {
            var attackForm = new AttackForm(sourceRegion, attackedRegion)
            {
                StartPosition = FormStartPosition.CenterParent
            };

            if (attackForm.ShowDialog(this) != DialogResult.OK)
                return -1;

            return attackForm.ArmyCount;
        }
    }
}
