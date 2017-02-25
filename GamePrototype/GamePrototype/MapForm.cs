using Common.Data;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GamePrototype
{
    public partial class MapForm : Form
    {
        private MapInfo _mapInfo;

        public MapForm(MapInfo mapInfo)
        {
            if (mapInfo == null || mapInfo.Info == null)
                throw new ArgumentNullException(); 

            InitializeComponent();

            _mapInfo = mapInfo;
        }

        private static readonly Random Rnd = new Random((int)DateTime.UtcNow.ToFileTime());

        public static MapInfo InitializeMapInfo(int color)
        {
            var result = new MapInfo {OwnColor = color};

            var info1 = new RegionInformation { LandId = 0xFF0000 };
            var info2 = new RegionInformation { LandId = 0x00FF00 };
            var info3 = new RegionInformation { LandId = 0x0000FF };

            const int widthCount = 10;
            const int heightCount = 10;
            result.Info = new RegionInformation[widthCount, heightCount];

            for (int wIndex = 0; wIndex < widthCount; ++wIndex)
            {
                for (int hIndex = 0; hIndex < heightCount; ++hIndex)
                {
                    switch (Rnd.Next(3))
                    {
                        case 0: result.Info[wIndex, hIndex] = info1; break;
                        case 1: result.Info[wIndex, hIndex] = info2; break;
                        case 2: result.Info[wIndex, hIndex] = info3; break;
                        default: throw new NotSupportedException("Random value");
                    }
                }
            }
            return result;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            int size = MapGenerator.RegionSize;
            int widthIndex = location.X/size;
            int heightIndex = location.Y/size;

            //            MessageBox.Show(string.Format("width index = {0}, height index = {1}",
            //                location.X / size + 1, location.Y / size + 1));

            RegionInformation regionInfo = _mapInfo.Info[widthIndex, heightIndex];
            bool ownRegion = (regionInfo.LandId == _mapInfo.OwnColor);

            if (ownRegion)
            {
                var chooseAction = new ChooseAction();
                if (chooseAction.ShowDialog() != DialogResult.OK)
                    return;

                regionInfo.LandId = 0x00FF00;
                Refresh();

                // [TODO]: Try implement pattern
                switch (chooseAction.Result)
                {
                    case ChosenAction.ShowInformation:
                        var infoForm = new RegionInformationForm(regionInfo);
                        infoForm.ShowDialog();
                        break;

                    case ChosenAction.CallReserve:
                        break;

                    case ChosenAction.Attack:
                        break;

                    case ChosenAction.Defend:
                        break;

                    case ChosenAction.JoinArmies:
                        break;

                    case ChosenAction.SplitArmy:
                        break;

                    case ChosenAction.MoveArmy:
                        break;

                    default:
                        throw new InvalidOperationException("Unknown option");
                }
            }
            else
            {
                var infoForm = new RegionInformationForm(regionInfo);
                infoForm.ShowDialog();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = Invite.GenerateMap(MapGenerator.RegionSize, _mapInfo.Info);
            e.Graphics.DrawImage(bmp, new Point(0, 0));
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Invite.GenerateMap(MapGenerator.RegionSize, _mapInfo.Info);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (var saveDlg = new SaveFileDialog())
            {
                if (saveDlg.ShowDialog() != DialogResult.OK)
                    return;

                MapInfoSerializer.Serialize(_mapInfo, saveDlg.FileName);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.FillRectangle(Invite.CreateBrush(_mapInfo.OwnColor), e.ClipRectangle);
            e.Graphics.DrawImage(bmp, new Point(0, 0));
        }
    }
}
