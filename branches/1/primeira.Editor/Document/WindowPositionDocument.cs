using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id = "FC5FDE83-71EE-4A3E-8694-465FBF719779",
                VersionNumber = "1.0",
                Name = "Window position & location cache",
                DefaultFileName = "Window Position",
                Description = "Stores the last position & location of an window.",
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
            DocumentHeader header = DocumentManager.RegisterDocument(typeof(WindowPositionDocument));

            string fileName = WindowName + ".windowcache";
            WindowPositionDocument doc = (WindowPositionDocument)DocumentManager.LoadDocument(
                header,
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
