using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id="F6A86949-950C-448C-9183-90DB3E3651D5",
                VersionNumber = "1.0",
                Name = "Addon Discovery Cache",
                DefaultFileName = "discovery",
                Description = "Stores the last addon discovery order.",
                DefaultFileExtension = ".cache",
                Options=DocumentHeaderOptions.OpenFromTypeDefaultName)]
    internal class AddonDiscoveryDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "discovery.cache"; }
        }

        [DataContract()]
        public class AssemblyTypeDocument
        {
            [DataMember()]
            public string AssemblyFile { get; set; }

            [DataMember()]
            public string Type { get; set; }

            public AssemblyTypeDocument(string assemblyFile, string type)
            {
                AssemblyFile = assemblyFile;
                Type = type;
            }
        }

        private List<AssemblyTypeDocument> _loadOrder = new List<AssemblyTypeDocument>();
      
        public void AddType(Type addonType)
        {
            AssemblyTypeDocument ass = new AssemblyTypeDocument(addonType.Assembly.CodeBase, addonType.Name);

            _loadOrder.Add(ass);
        }

        public void Clear()
        {
            _loadOrder.Clear();
        }

        [DataMember()]
        public AssemblyTypeDocument[] LoadOrder
        {
            get { return _loadOrder.ToArray(); }
            set
            {
                if (_loadOrder == null)
                    _loadOrder = new List<AssemblyTypeDocument>(value.Length);
                else
                    _loadOrder.Clear();

                _loadOrder.AddRange(value);
            }
        }

        public static AddonDiscoveryDocument GetInstance()
        {
            DocumentHeader def = DocumentManager.RegisterDocument(typeof(AddonDiscoveryDocument));

            AddonDiscoveryDocument doc = (AddonDiscoveryDocument)DocumentManager.LoadDocument(def);

            if (doc == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.DocumentCreationError, FileName));

            return doc;
        }

        public void ToXml()
        {
            DocumentManager.SaveDocument(this);
        }

    }
}
