using System;
using System.Windows.Forms;

namespace GamePrototype
{
    public enum ChosenAction
    {
        None = 0,
        Attack,
        Defend,
        MoveArmy,
        JoinArmies,
        SplitArmy,
        CallReserve,
        ShowInformation
    }

    public partial class ChooseAction : Form
    {
        public ChooseAction()
        {
            InitializeComponent();
        }

        public ChosenAction Result
        {
            get
            {
                if (getReserve.Checked)
                    return ChosenAction.CallReserve;
                if (attackNearRegion.Checked)
                    return ChosenAction.Attack;
                if (defendRegion.Checked)
                    return ChosenAction.Defend;
                if (moveArmy.Checked)
                    return ChosenAction.MoveArmy;
                if (joinArmies.Checked)
                    return ChosenAction.JoinArmies;
                if (splitArmy.Checked)
                    return ChosenAction.SplitArmy;
                if (regionInfo.Checked)
                    return ChosenAction.ShowInformation;
                return ChosenAction.None;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
//            Result = true;
//            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
//            Result = false;
//            Close();
        }
    }
}
