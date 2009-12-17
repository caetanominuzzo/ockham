using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace primeira.Editor.Business
{

    public delegate void SelectedDelegate(IEditor sender);

    public static class EditorManager
    {
        private static Type[] _defaultEditorCtor = new Type[2] { typeof(string), typeof(DocumentBase) };

        private static IEditor CreateEditorByFilename(string filename)
        {
            IEditor res = null;
            DocumentBase doc = null;

            FileInfo f = new FileInfo(filename);

            if (!f.Exists)
            {
                f.Create();
                f.Refresh();
            }

            if (f.Length == 0)
            {
                string ext = Path.GetExtension(filename);

                Type tt = DocumentManager.GetDocumentDefinitionByFileExtension(ext).DefaultEditor;

                if (tt == null)
                    return null;

                res = (IEditor)tt.GetConstructor(_defaultEditorCtor).Invoke(new object[2] { filename, doc });
            }
            else
            {
                doc = DocumentManager.ToObject(filename);

                if (doc != null)
                    res = (IEditor)doc.Definition.DefaultEditor.GetConstructor(_defaultEditorCtor).Invoke(new object[2] { filename, doc });
            }

            return (IEditor)res;
        }
 
        public static void RegisterEditors()
        {
            string pluginDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");

            string[] dlls = Directory.GetFiles(pluginDir, "*.dll", SearchOption.AllDirectories);

            Assembly ass = null;
            foreach (string dll in dlls)
            {
                ass = Assembly.LoadFile(dll);

                Type[] types = ass.GetTypes();


                foreach (Type type in types)
                {
                    if (type.BaseType == typeof(DocumentBase))
                        DocumentManager.RegisterDocument(type);
                }
            }
        }
        
        public static IEditor LoadEditor(string filename)
        {
            IEditor res = TabManager.GetInstance().GetDocumentByFilename(filename);

            if (res != null)
            {
                res.Selected = true;
                return res;
            }

            if (res == null)
            {
                res = EditorManager.CreateEditorByFilename(filename);

                if (res != null && (res.Document.Definition.Options & DocumentDefinitionOptions.TabControl) != DocumentDefinitionOptions.TabControl)
                    DocumentManager.AddDocument(res);
            }

            return res;
        }

    }
}
