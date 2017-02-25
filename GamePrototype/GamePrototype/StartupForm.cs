using System;
using System.Windows.Forms;

namespace GamePrototype
{
    public enum LoadType
    {
        None = 0,
        NewGame,
        LoadGame
    }

    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        public LoadType LoadType { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.LoadType = newGame.Checked ? LoadType.NewGame : LoadType.LoadGame;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
