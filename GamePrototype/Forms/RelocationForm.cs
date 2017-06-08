using System;
using System.Windows.Forms;

namespace Forms
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
            trackBar1.Minimum = 0;
            trackBar1.Maximum = FromCount;

            fromCountBox.Minimum = 0;
            fromCountBox.Maximum = FromCount;

            toCountBox.Minimum = ToCount;
            toCountBox.Maximum = ToCount + FromCount;

            // === Initialize ===

            trackBar1.Value = 0;
            fromCountBox.Value = FromCount;
            toCountBox.Value = ToCount;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FromCount -= trackBar1.Value;
            ToCount += trackBar1.Value;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int val = ((TrackBar) sender).Value;
            if (fromCountBox.Value != FromCount - val)
            {
                fromCountBox.Value = FromCount - val;
            }
            if (toCountBox.Value != ToCount + val)
            {
                toCountBox.Value = ToCount + val;
            }
        }

        private void fromCountBox_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = FromCount - (int)fromCountBox.Value;
        }

        private void toCountBox_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)toCountBox.Value - ToCount;
        }

        private void fromCountBox_KeyUp(object sender, KeyEventArgs e)
        {
            decimal result;
            if (decimal.TryParse(fromCountBox.Text, out result))
            {
                if (result > fromCountBox.Maximum)
                    result = fromCountBox.Maximum;
                if (result < fromCountBox.Minimum)
                    result = fromCountBox.Minimum;

                fromCountBox.Value = result;
            }
        }

        private void toCountBox_KeyUp(object sender, KeyEventArgs e)
        {
            decimal result;
            if (decimal.TryParse(toCountBox.Text, out result))
            {
                if (result > toCountBox.Maximum)
                    result = toCountBox.Maximum;
                if (result < toCountBox.Minimum)
                    result = toCountBox.Minimum;

                toCountBox.Value = result;
            }
        }
    }
}
