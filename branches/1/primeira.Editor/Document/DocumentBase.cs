﻿using System;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class DocumentBase
    {
        private DocumentDefinition _definition = null;

        public DocumentDefinition Definition
        {
            get
            {
                if (_definition == null)
                    _definition = DocumentManager.RegisterDocument(this.GetType());

                return _definition;
            }
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
    }
}
