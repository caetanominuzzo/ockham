using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Drawing;

namespace primeira.Editor
{
    /// <summary>
    /// Provides methods to register and manage editors headers and editors. 
    /// 
    /// Editors are classes defined in assemblies with EditorHeader Attribute.
    /// 
    /// All editors are dicovered and registered by the application after the addons discovery.
    /// 
    /// Like AddonManager & DocumentManager this class works with a cache of headers (here, editor headers).
    /// This cache is usefeul to prevents intensive access to custom attributes.
    /// Like AddonManager, but unlike DocumentManager classes EditorManager persists this cache on file.
    /// This cache file is an EditorHeaderDocument object.
    /// </summary>
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
                            b.DocumentVersions = GetEditorDocumentsHeadersFromReflection(b);
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

                    header.DocumentVersions = GetEditorDocumentsHeadersFromReflection(header);
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
        /// Registers a System.Type as an editor.
        /// </summary>
        /// <param name="editorType">The System.Type to register</param>
        public static EditorHeader GetEditorHeader(VersionFilter version)
        {
            return ( from a in cache.Headers
                     where a.Version != null && a.Version.Filter(version)
                     orderby a.Version
                     select a ).FirstOrDefault();
        }

        /// <summary>
        /// Loads the specified file in the registered editor.
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

        private static IEditor InternalLoadEditor(string fileName)
        {
            IEditor res = null;

            Type editorType;

            DocumentHeader header = DocumentManager.GetDocumentHeader(fileName);

            editorType = ( from a in EditorManager.Headers.AsParallel()
                           where a.DocumentVersions != null && header.Version.FilterAny(a.DocumentVersions)
                           select a.BaseType ).FirstOrDefault();

            if (editorType == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.ThereIsNoEditorForType, fileName));
            
            res = (IEditor)editorType.GetConstructor(_defaultEditorCtor).Invoke(new object[1] { fileName });

            return res;

        }

        private static EditorHeader GetEditorHeaderFromReflection(Type editorType)
        {
            EditorHeader editor = null;

            EditorHeaderAttribute attrib = (EditorHeaderAttribute)editorType.Assembly.GetCustomAttributes(typeof(EditorHeaderAttribute), false).FirstOrDefault();

            if (attrib != null)
            {
                editor = new EditorHeader(editorType);

                editor.GetHeaderBaseFromReflection(attrib);

                
            }

            return editor;
        }

        private static VersionFilter[] GetEditorDocumentsHeadersFromReflection(EditorHeader header)
        {
            EditorDocumentAttribute[] dd = (EditorDocumentAttribute[])header.BaseType.GetCustomAttributes(typeof(EditorDocumentAttribute), false);

            List<VersionFilter> tmp = new List<VersionFilter>(dd.Length);

            DocumentHeader doc;

            foreach (EditorDocumentAttribute d in dd)
            {
                doc = DocumentManager.RegisterDocument(d.DocumentType);

                if (doc.DefaultEditor == null || header.Version > doc.DefaultEditor.Version)
                    doc.DefaultEditor = header;

                tmp.Add((VersionFilter)doc.Version);
            }

            return tmp.ToArray();
        }

        private static Type[] _defaultEditorCtor = new Type[1] { typeof(string) };
    }
}
