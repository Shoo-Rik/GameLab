using System;
using System.Drawing;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Show startup form

            var startupForm = new StartupForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (startupForm.ShowDialog() != DialogResult.OK)
                return;

            // [TODO]: Move to config
            int color = Color.Red.ToArgb();

            // 2. Get model

            Model model = null;

            if (startupForm.LoadType == LoadType.NewGame)
            {
                model = new Model(color);
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
