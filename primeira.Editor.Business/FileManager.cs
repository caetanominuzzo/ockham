﻿using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace primeira.Editor.Business
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

        public static string GetNewFile(DocumentDefinition fileVersion, string basedir)
        {
            string result = string.Empty;

            for (int i = 1; i < 100; i++)
            {
                string name = string.Format(fileVersion.DefaultName, i);

                result = string.Format("{0}{1}", name, fileVersion.Extension);

                if (!File.Exists(string.Format("{0}{1}", Path.Combine(basedir, name), fileVersion.Extension)))
                    break;
            }
            return result;
        }

        public static void MeasureFromIDC(Button button)
        {
            string value = button.Text;

            Font f = button.Font;
            Size Size = new Size(
                        button.Width - (button.Padding.Left + button.Padding.Right),
                        button.Height - (button.Padding.Top+ button.Padding.Bottom));

            char[] ss = new char[value.Length];

            value.CopyTo(0, ss, 0, value.Length);

            string s = new string(ss);

            TextFormatFlags t = TextFormatFlags.ModifyString | TextFormatFlags.PathEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;

            TextRenderer.MeasureText(button.CreateGraphics(), s, f, Size, t);
            int i = s.IndexOf("\0");
            if (i > -1)
                s = s.Substring(0, i);

            button.Text = s.Replace(Path.GetExtension(s), "");
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