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
    public class DocumentBase 
    {
        #region Serialization

        /// <summary>
        /// Deserializes a given file in an specified document type.
        /// </summary>
        /// <param name="fileName">A file path</param>
        /// <param name="type">The type of the document</param>
        /// <returns></returns>
        protected static DocumentBase ToObject(string fileName, Type type)
        {
            return DocumentManager.ToObject(fileName, type);

        }

        /// <summary>
        /// Serializes a given document in an specified file.
        /// </summary>
        /// <param name="document">The document to serialize</param>
        /// <param name="fileName">The file to serialize</param>
        protected static void ToXml(DocumentBase document, string fileName)
        {
            DocumentManager.ToXml(document, fileName);
        }

        #endregion
    }
}
