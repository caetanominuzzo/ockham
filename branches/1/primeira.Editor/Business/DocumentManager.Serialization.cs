using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    partial class DocumentManager
    {
        /// <summary>
        /// Loads a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        /// <returns>A loaded document</returns>
        public static DocumentBase LoadDocument(DocumentHeader document, string fileName)
        {
            if (document.Options.HasFlag(DocumentHeaderOptions.CustomSerializationRead))
            {
                MethodInfo m = document.BaseType.GetMethod("ToObject", new Type[] { typeof(string) });

                if (m == null)
                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializationReadMustHaveToObjectMethod);

                try
                {
                    return (DocumentBase)m.Invoke(null, new object[] { fileName });
                }
                catch (Exception ex)
                {
                    LogFileManager.Log(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, document.Name), Environment.NewLine, ex.ToString());

                    throw new InvalidOperationException(
                        string.Format(
                            Message_en.DocumentCustomSerializatinoError, document.Name), ex);
                }
            }

            return DocumentManager.InternalLoadDocument(document.BaseType, fileName);
        }

        /// <summary>
        /// Saves a document by its default name.
        /// </summary>
        /// <param name="documentType">The System.Type of the document to open</param>
        /// <param name="fileName">The file to load</param>
        public static void SaveDocument(DocumentBase document, string fileName)
        {
            if (document.Header.Options.HasFlag(DocumentHeaderOptions.CustomSerializationWrite))
            {
                MethodInfo m = document.GetType().GetMethod("ToXml", new Type[] { typeof(string) });

                if (m == null)
                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializationWriteMustHaveToObjectMethod);

                try
                {
                    m.Invoke(document, new object[] { fileName });
                }
                catch (Exception ex)
                {
                    LogFileManager.Log(Message_en.DocumentCustomSerializatinoError);

                    throw new InvalidOperationException(
                        Message_en.DocumentCustomSerializatinoError, ex);
                }

                return;
            }

            InternalSaveDocument(document, fileName);

        }

        /// <summary>
        /// Deserializes a given file in an specified document type.
        /// </summary>
        /// <param name="type">The type of the document</param>
        /// <param name="fileName">A file path</param>
        /// <returns></returns>
        internal static DocumentBase InternalLoadDocument(Type type, string fileName)
        {
            FileInfo f = new FileInfo(fileName);

            DocumentBase doc = null;

            if (!f.Exists || f.Length == 0)
            {
                doc = (DocumentBase)type.GetConstructor(System.Type.EmptyTypes).Invoke(System.Type.EmptyTypes);
            }
            else
            {
                Stream sm = File.OpenRead(fileName);

                Type[] knownTypes = null;
                if (_headers != null && Headers.Length > 0)
                    knownTypes = ( from a in Headers
                                   where a.BaseType != null
                                   select a.BaseType ).ToArray();
                else
                    knownTypes = new Type[1] { type};

                DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                    knownTypes,
                    10000000, false, true, null);

                doc = (DocumentBase)ser.ReadObject(sm);

                UndoRedoFramework.UndoRedoManager.FlushHistory();

                sm.Close();
            }

            return doc;
        }

        /// <summary>
        /// Serializes a given document in an specified file.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        /// <param name="fileName">The file to serialize</param>
        internal static void InternalSaveDocument(DocumentBase document, string fileName)
        {

            Stream sm = File.Create(fileName);

            Type[] knownTypes = ( from a in Headers
                                  where a.BaseType != null
                                  select a.BaseType ).ToArray();

            DataContractSerializer ser = new DataContractSerializer(typeof(DocumentBase),
                knownTypes,
                10000000, false, true, null);
            ser.WriteObject(sm, document);

            sm.Close();
        }        
    }
}
