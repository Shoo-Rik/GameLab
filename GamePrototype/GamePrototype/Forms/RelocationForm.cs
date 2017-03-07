using System;
using System.Windows.Forms;

namespace GamePrototype.Forms
{
    public partial class RelocationForm : Form
    {
        public int FromCount { get; private set; }
        public int ToCount { get; private set; }

        public RelocationForm(int fromCount, int toCount)
        {
            InitializeComponent();

            FromCount = fromCount;
            ToCount = toCount;
        }

        private void RelocationForm_Load(object sender, EventArgs e)
        {
            fromCountBox.Text = $"{FromCount}";
            toCountBox.Text = $"{ToCount}";
            trackBar1.Minimum = 0;
            trackBar1.Maximum = FromCount;
            trackBar1.Value = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            fromCountBox.Text = $"{FromCount - ((TrackBar)sender).Value}";
            toCountBox.Text = $"{ToCount + ((TrackBar)sender).Value}";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FromCount -= trackBar1.Value;
            ToCount += trackBar1.Value;
        }
    }
}
