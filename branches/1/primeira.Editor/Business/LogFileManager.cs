using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class LogFileManager
    {
        private static string logpath;

        public static void Log(params string[] message)
        {
            if(logpath == null)
            {
                logpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Application.ProductName) + ".log";
            }

            string msg =  string.Concat(message);

            File.AppendAllText(logpath,
                string.Concat(
                DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture),
                " - ",
                msg,
                Environment.NewLine));
        }
    }
}
