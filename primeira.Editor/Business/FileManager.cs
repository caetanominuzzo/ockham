using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace primeira.Editor
{
    public static partial class FileManager
    {
        public static string LastWrite(TimeSpan time)
        {
            if(DateTime.Now.Subtract(time).Day == DateTime.Now.AddDays(-1).Day)
            // if ((int)time.TotalDays == 1)
                return "Yesterday";
            else if (time.TotalDays >= 2)
                return string.Format("{0} days ago", (int)time.TotalDays);

            else if ((int)time.TotalHours == 1)
                return string.Format("One hour ago");
            else if (time.TotalHours >= 2)
                return string.Format("{0} hours ago", (int)time.TotalHours);

            else if (time.TotalMinutes >= 2)
                return string.Format("{0} minutes ago", (int)time.TotalMinutes);
            else
                return "recently saved";
        }

        public static string GetNewFile(DocumentDefinitionAttribute fileVersion, string basedir)
        {
            string result = string.Empty;

            for (int i = 1; i < 100; i++)
            {
                string name = string.Format(fileVersion.DefaultFileName, i);

                result = string.Format("{0}{1}", name, fileVersion.DefaultFileExtension);

                if (!File.Exists(string.Format("{0}{1}", Path.Combine(basedir, name), fileVersion.DefaultFileExtension)))
                    break;
            }
            return result;
        }

        #region Recent file

        public static IRecentFileControl Recent { get; private set; }

        public static void SetRecentManager(IRecentFileControl recentManager)
        {
            Recent = recentManager;
        }

        #endregion

    }
}
