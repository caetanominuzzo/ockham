using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
        [DocumentDefinition(Name = "Application Log",
                DefaultFileName = "application",
                Description = "Stores the application log.",
                DefaultFileExtension = ".log",
                Options = DocumentDefinitionOptions.OpenFromTypeDefaultName |
                 DocumentDefinitionOptions.CustomSerialization)]
    public class ApplicationLogDocument : DocumentBase
    {
        internal class ApplicationLogItem
        {
            public DateTime DateTime { get; private set; }
            public string Message { get; private set; }

            public ApplicationLogItem(DateTime dateTime, string message)
            {
                this.DateTime = dateTime;
                this.Message = message;
            }

            public string ToString()
            {
                return string.Concat(
                    this.DateTime.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ": ",
                    Message);
            }

        }

        private string fileName;

        public static DocumentBase ToObject(string fileName)
        {
            ApplicationLogDocument log = new ApplicationLogDocument();
            log.fileName = fileName;

            return log;
        }

        public void AddLog(string message)
        {
            System.IO.File.AppendAllText(this.fileName, message);
        }

        private ApplicationLogDocument()
        {
        }
    }
}
