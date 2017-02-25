using System;
using System.Windows.Forms;
using Common.Data;

namespace GamePrototype
{
    public partial class RegionInformationForm : Form
    {
        private RegionInformation _info;

        public RegionInformationForm(RegionInformation info)
        {
            InitializeComponent();

            _info = info;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
