using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Engine;
using Forms;

namespace GamePrototype
{
    static class Program
    {
        // ReSharper disable once NotAccessedField.Local
        private static Mutex _appMutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool canRun;
            _appMutex = new Mutex(true, "WorldManagement", out canRun);
            if (!canRun)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Show startup form

            var startupForm = new StartupForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (startupForm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Color ownColor = startupForm.OwnColor;
            Color enemy1Color = startupForm.Enemy1Color;
            Color enemy2Color = startupForm.Enemy2Color;

            // 2. Get model

            Model model = null;

            if (startupForm.LoadType == LoadType.NewGame)
            {
                model = new Model(ownColor, enemy1Color, enemy2Color);
            }
            if (startupForm.LoadType == LoadType.LoadGame)
            {
                using (var openDlg = new OpenFileDialog())
                {
                    if (openDlg.ShowDialog() != DialogResult.OK)
                        return;

                    model = new Model(openDlg.FileName);
                }
            }

            if (model == null)
                throw new NullReferenceException("model");

            // 3. Show main form

            var mainForm = new MapForm(model)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            Application.Run(mainForm);
        }
    }
}
