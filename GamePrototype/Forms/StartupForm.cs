using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Forms
{
    public enum LoadType
    {
        None = 0,
        NewGame,
        LoadGame
    }

    public partial class StartupForm : Form
    {
        private readonly IDictionary<string, Color> _mColors;

        public StartupForm()
        {
            InitializeComponent();

            _mColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            foreach (PropertyInfo c in typeof(Color).GetProperties())
            {
                if (typeof (Color).IsAssignableFrom(c.PropertyType))
                {
                    _mColors.Add(c.Name, (Color)c.GetValue(null));
                }
            }
        }

        public Color OwnColor => _mColors[ownColorComboBox.Text];

        public Color Enemy1Color => _mColors[enemy1ColorComboBox.Text];

        public Color Enemy2Color => _mColors[enemy2ColorComboBox.Text];

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ownColorComboBox.DataSource = new List<string>(_mColors.Keys);
            ownColorComboBox.Text = "Red";

            enemy1ColorComboBox.DataSource = new List<string>(_mColors.Keys);
            enemy1ColorComboBox.Text = "Green";

            enemy2ColorComboBox.DataSource = new List<string>(_mColors.Keys);
            enemy2ColorComboBox.Text = "DarkOrange";
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

        private void newGame_CheckedChanged(object sender, EventArgs e)
        {
            if (newGame.Checked)
            {
                groupBox2.Enabled = true;
                label1.Enabled = true;
                label2.Enabled = true;
                label3.Enabled = true;
                ownColorComboBox.Enabled = true;
                enemy1ColorComboBox.Enabled = true;
                enemy2ColorComboBox.Enabled = true;
                pictureBox1.Enabled = true;
                pictureBox2.Enabled = true;
                pictureBox3.Enabled = true;
            }
        }

        private void loadGame_CheckedChanged(object sender, EventArgs e)
        {
            if (loadGame.Checked)
            {
                groupBox2.Enabled = false;
                label1.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
                ownColorComboBox.Enabled = false;
                enemy1ColorComboBox.Enabled = false;
                enemy2ColorComboBox.Enabled = false;
                pictureBox1.Enabled = false;
                pictureBox2.Enabled = false;
                pictureBox3.Enabled = false;
            }
        }

        private static Bitmap GetBitmap(PictureBox pictureBox, string colorName)
        {
            var bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(new SolidBrush(Color.FromName(colorName)), new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
            return bitmap;
        }

        private void ownColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = GetBitmap(pictureBox1, ownColorComboBox.Text);
        }

        private void enemy1ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox2.Image = GetBitmap(pictureBox2, enemy1ColorComboBox.Text);
        }

        private void enemy2ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox3.Image = GetBitmap(pictureBox3, enemy2ColorComboBox.Text);
        }
    }
}
