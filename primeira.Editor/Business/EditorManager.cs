using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Drawing;

namespace primeira.Editor
{
    public delegate void SelectedDelegate(IEditor sender);

    public static class EditorManager
    {
        private static EditorHeaderDocument cache = null;

        public static EditorHeader[] Headers
        {
            get
            {
                return cache.Headers;
            }
        }

        public static void Discovery()
        {
            cache = EditorHeaderDocument.GetInstance();

            if (cache.LastWriteTime > AddonManager.CacheLastWriteTime)
            {
                AddonManager.Addons.AsParallel().ForAll(a =>
                {
                    cache.Headers.AsParallel().ForAll(b =>
                    {
                        if (a.Version.Id == b.Version.Id)
                        {
                            b.BaseType = a.BaseType;
                            b.DocumentVersions = GetEditorDocumentsHeadersFromReflection(b.BaseType);
                        }
                    });
                });
            }
            else
            {
                cache.Clear();

                AddonManager.Addons.AsParallel().ForAll(a =>
                {
                    EditorHeader header = null;

                    header = GetEditorHeaderFromReflection(a.BaseType);

                    if (header != null)
                        cache.AddEditorHeader(header);
                });

                cache.ToXml();
            }
        }

        /// <summary>
        /// Registers a System.Type as an editor.
        /// </summary>
        /// <param name="editorType">The System.Type to register</param>
        public static EditorHeader GetEditorHeader(Type editorType)
        {
            return cache.Headers.AsParallel().Where(x => x.BaseType == editorType).FirstOrDefault();
        }
        
        /// <summary>
        /// Registers a System.Type as an editor.
        /// </summary>
        /// <param name="editorType">The System.Type to register</param>
        public static EditorHeader[] GetEditorHeaders(VersionFilter version)
        {
            return cache.Headers.Where(x => x.Version.Id == version.Target).ToArray();
        }

        /// <summary>
        /// Load the specified file in the registered editor.
        /// If the file is already open its tab is selected.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        /// <returns>The editor with file loaded.</returns>
        public static IEditor LoadEditor(string fileName)
        {
            //Verify if the file is already open
            IEditor res = EditorContainerManager.GetOpenEditor(fileName);
            if (res != null)
            {
                EditorContainerManager.AddEditor(res);
                return res;
            }

            try
            {
                res = EditorManager.InternalLoadEditor(fileName);

                if (res != null)
                    EditorContainerManager.AddEditor(res);

                return res;
            }
            catch (TargetInvocationException ex)
            {
                LogFileManager.Log(
                    string.Format(
                        Message_en.EditorCreationError,
                        fileName),
                        Environment.NewLine,
                        ex.ToString());

                throw;
            }

        }

        /// <summary>
        /// Load the specified document type with default fileName.
        /// The document must has OpenFromTypeByDefaultName option.
        /// </summary>
        /// <param name="documentType">The CLR type of the document to open.</param>
        /// <returns>The editor with the default file loaded.</returns>
        public static IEditor LoadEditor(DocumentHeader document)
        {
            if (document.Options.HasFlag(DocumentHeaderOptions.OpenFromTypeDefaultName))
                return LoadEditor(document.DefaultFileName + document.DefaultFileExtension);

            throw new InvalidOperationException(Message_en.DocumentMissingOpenFromTypeDefaultName);
        }

        private static EditorHeader GetEditorHeaderFromReflection(Type editorType)
        {
            EditorHeader editor = null;

            EditorHeaderAttribute attrib = (EditorHeaderAttribute)editorType.Assembly.GetCustomAttributes(typeof(EditorHeaderAttribute), false).FirstOrDefault();

            if (attrib != null)
            {
                editor = new EditorHeader(editorType);

                editor.GetHeaderBaseFromReflection(attrib);

                editor.DocumentVersions = GetEditorDocumentsHeadersFromReflection(editorType);
            }

            return editor;
        }

        private static VersionData[] GetEditorDocumentsHeadersFromReflection(Type editorType)
        {
            EditorDocumentAttribute[] dd = (EditorDocumentAttribute[])editorType.GetCustomAttributes(typeof(EditorDocumentAttribute), false);

            List<VersionData> tmp = new List<VersionData>(dd.Length);

            foreach (EditorDocumentAttribute d in dd)
            {
                tmp.Add(DocumentManager.RegisterDocument(d.DocumentType).Version);
            }

            return tmp.ToArray();
        }

        private static IEditor InternalLoadEditor(string fileName)
        {
            IEditor res = null;

            Type editorType;

            DocumentHeader header = DocumentManager.GetDocumentHeader(fileName);

            editorType = ( from a in EditorManager.Headers.AsParallel()
                           where a.DocumentVersions != null && a.DocumentVersions.Contains(header.Version)
                           select a.BaseType ).FirstOrDefault();

            if (editorType == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.ThereIsNoEditorForType, fileName));


            res = (IEditor)editorType.GetConstructor(_defaultEditorCtor).Invoke(new object[1] { fileName });

            return res;

        }

        private static Type[] _defaultEditorCtor = new Type[1] { typeof(string) };


    }
}
