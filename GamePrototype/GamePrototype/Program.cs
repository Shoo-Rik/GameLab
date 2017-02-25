using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Data;

namespace GamePrototype
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Test

            /*var info1 = new RegionInfo { LandId = 0xFF0000 };
            var info2 = new RegionInfo { LandId = 0x0000FF };

            RegionInfo[,] infos = new RegionInfo[2, 3]
            {
                { info1, info1, info2 },
                { info1, info2, info2 },
            };

            string filePath = @"C:\Projects\Temp\aaa.xml";

            MapInfoSerializer.Serialize(infos, filePath);

            RegionInfo[,] res = MapInfoSerializer.Deserialize(filePath);*/

            #endregion Test

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var startupForm = new StartupForm();
            if (startupForm.ShowDialog() != DialogResult.OK)
                return;

            MapInfo info = null;
            int color = 0xFF0000;

            if (startupForm.LoadType == LoadType.NewGame)
            {
                info = MapForm.InitializeMapInfo(color);
            }
            if (startupForm.LoadType == LoadType.LoadGame)
            {
                using (var openDlg = new OpenFileDialog())
                {
                    if (openDlg.ShowDialog() != DialogResult.OK)
                        return;

                    info = MapInfoSerializer.Deserialize(openDlg.FileName);
                }
            }

            if (info == null)
                throw new NullReferenceException("info");

            Application.Run(new MapForm(info));
        }
    }
}
