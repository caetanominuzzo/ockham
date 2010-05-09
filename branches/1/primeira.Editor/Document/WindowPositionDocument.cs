using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Window position & location cache",
                DefaultFileName = "Window Position",
                Description = "Stores the last position & location og an window.",
                DefaultFileExtension = ".windowcache")]
    public class WindowPositionDocument : DocumentBase
    {
        [DataMember()]
        public Point Location { get; set; }

        [DataMember()]
        public Size Size { get; set; }

        [DataMember()]
        public System.Windows.Forms.FormWindowState WindowState { get; set; }

        public static WindowPositionDocument GetInstance(string WindowName)
        {
            DocumentDetail detail = DocumentManager.RegisterDocument(typeof(WindowPositionDocument));

            string fileName = WindowName + ".windowcache";
            WindowPositionDocument doc = (WindowPositionDocument)DocumentManager.LoadDocument(
                detail,
                fileName);

            if (doc == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.DocumentCreationError, fileName));

            return doc;
        }

        public void Save(string WindowName)
        {
            string fileName = WindowName + ".windowcache";
         
            DocumentManager.SaveDocument(
                this,
                fileName);
        }

        
    }
}
