using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace primeira.Editor
{
    public static partial class DocumentManager
    {

        private static List<DocumentHeader> _headers = new List<DocumentHeader>(); 
        
        private static string _baseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string BaseDir
        {
            get { return _baseDir; }
            set { _baseDir = value; }
        }

        public static DocumentHeader[] Headers
        {
            get
            {
                return _headers.ToArray();
            }
        }

        /// <summary>
        /// Registers a System.Type as a document.
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static DocumentHeader RegisterDocument(Type documentType)
        {
            DocumentHeader header = DocumentManager.GetDocumentHeader(documentType);

            if (header == null)
            {
                header = DocumentManager.GetDocumentHeaderFromReflection(documentType);

                if (_headers == null)
                    _headers = new List<DocumentHeader>();

                _headers.Add(header);
            }

            return header;
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file.
        /// </summary>
        /// <param name="fileName">Path to a file</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentHeader GetDocumentHeader(string fileName)
        {
            DocumentHeader[] document = GetDocumentHeaderByFileExtension(Path.GetExtension(fileName));

            if (document.Length == 1)
                return document[0];
            else
            {
                throw new NotImplementedException();

                //TODO: Read document header from file.
                //Read the begining (a chunk of bytes) of the given file.
            }
        }

        /// <summary>
        /// Gets the System.Type of the document of a given file extension.
        /// </summary>
        /// <param name="extension">File extesion</param>
        /// <returns>The System.Type of the document of a given file</returns>
        public static DocumentHeader[] GetDocumentHeaderByFileExtension(string extension)
        {
            return ( from a in Headers
                     where a.DefaultFileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase)
                     select a ).ToArray();
        }

        /// <summary>
        /// Gets the DocumentHeaderAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentHeaderAttribute of the given System.Type</returns>
        private static DocumentHeader GetDocumentHeader(Type documentType)
        {
            return ( from a in Headers.AsParallel()
                     where a.BaseType == documentType
                     select a ).FirstOrDefault();
        }

        /// <summary>
        /// Gets the DocumentHeaderAttribute of a given document System.Type.
        /// </summary>
        /// <param name="documentType">The type of a document</param>
        /// <returns>The DocumentHeaderAttribute of the given System.Type</returns>
        private static DocumentHeader GetDocumentHeader(VersionData version)
        {
            return ( from a in Headers.AsParallel()
                     where a.Version == version
                     select a ).FirstOrDefault();
        }

        private static DocumentHeader GetDocumentHeaderFromReflection(Type documentType)
        {
            object[] attribs = documentType.GetCustomAttributes(typeof(DocumentHeaderAttribute), false);

            if (attribs.Length != 0)
            {
                DocumentHeaderAttribute attr       = (DocumentHeaderAttribute)attribs[0];

                DocumentHeader header              = new DocumentHeader();

                header.GetHeaderBaseFromReflection(attr);

                header.BaseType = documentType;

                header.Options = attr.Options;
                header.DefaultFileName = attr.DefaultFileName;
                header.DefaultFileExtension = attr.DefaultFileExtension;
                header.FriendlyNameMask = header.FriendlyNameMask;

                header.Icon = DocumentManager.GetIcon(documentType, attr.IconResourceFile);

                header.Version = new VersionData(attr.Id, attr.VersionNumber, attr.Author, attr.Info, attr.Email, attr.WebSite);

                if (header.DefaultEditorVersion != null)
                {
                    EditorHeader[] editor = EditorManager.GetEditorHeaders(header.DefaultEditorVersion);

                    if (editor.Count() > 0)
                        header.DefaultEditor = editor[0];
                }

                return header;
            }
            else
                throw new InvalidOperationException(string.Format(Message_en.DocumentMissingDocumentHeaderAttribute,
                    documentType.Name));
        }

        private static Image GetIcon(Type type, string iconResourceFile)
        {
            if (iconResourceFile == null || iconResourceFile.Length == 0)
                iconResourceFile = "File.ico";

            Stream stream = type.Assembly.GetManifestResourceStream(
                string.Concat(type.Namespace, ".Resources.", iconResourceFile));

            if (stream == null)
                return null;

            return Image.FromStream(stream);
        }

    }
}
