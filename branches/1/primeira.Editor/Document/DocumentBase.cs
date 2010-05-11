using System;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentBase : IExtensibleDataObject
    {
        private DocumentHeader _header = null;

        [DataMember(Order=0)]
        public DocumentHeader Header
        {
            get
            {
                if (_header == null)
                    _header = DocumentManager.RegisterDocument(this.GetType());

                return _header;
            }
            set { } //Just for serialization. The header must be registered in the getter.
        }

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

        #region IExtensibleDataObject Members

        private ExtensionDataObject _extensionDataObject;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return _extensionDataObject;
            }
            set
            {
                _extensionDataObject = value;
            }
        }

        #endregion
    }
}
