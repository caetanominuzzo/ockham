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
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            AddonManager.Discovery();

            EditorManager.Discovery();

            AddonManager.InitializeAddons();

            ShortcutManager.InitializePreFilter();

            Application.Run(EditorContainerManager.MainMForm);
        }
    }
}
