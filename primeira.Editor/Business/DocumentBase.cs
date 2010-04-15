using System;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using System.Reflection.Emit;
using System.Threading;

namespace primeira.Editor
{
    [DataContract()]
    public abstract class DocumentBase 
    {
        public void ToXml(string filename)
        {
            Stream sm = File.Create(filename);

            Type[] knownTypes = DocumentManager.GetKnownDocumentTypes();

            Array.Resize(ref knownTypes, knownTypes.Length + 1);

            knownTypes[knownTypes.Length - 1] = this.GetType();

            DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                knownTypes,
                10000000, false, true, null);
            ser.WriteObject(sm, this);

            sm.Close();
        }

        public static DocumentBase ToObject(string filename, Type type)
        {
            try
            {

                Stream sm = File.OpenRead(filename);

                Type[] knownTypes = DocumentManager.GetKnownDocumentTypes();

                Array.Resize(ref knownTypes, knownTypes.Length + 1);

                knownTypes[knownTypes.Length - 1] = type;

                DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                    DocumentManager.GetKnownDocumentTypes(),
                    10000000, false, true, null);

                DocumentBase res = (DocumentBase)ser.ReadObject(sm);

                UndoRedoFramework.UndoRedoManager.FlushHistory();

                sm.Close();

                return res;
            }
            catch(Exception ex)
            {
                MessageManager.Alert("There is an error while deserializing file ", filename, ".\n", ex.Message);
            }
            
            return null;
        }
    }
}
