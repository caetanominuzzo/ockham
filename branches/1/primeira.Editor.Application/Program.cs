using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace primeira.Editor
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

            AddonManager.Discovery();

            ShortcutManager.InitializePreFilter();

            Application.Run(EditorContainerManager.MainMForm);
        }
    }
}
