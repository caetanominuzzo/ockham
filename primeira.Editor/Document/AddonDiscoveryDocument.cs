using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentDefinition(Name = "Addon Discovery Cache",
                DefaultFileName = "discovery",
                Description = "Stores the last addon discovery order.",
                DefaultFileExtension = ".cache",
                Options=DocumentDefinitionOptions.OpenFromTypeDefaultName)]
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
            AddonDiscoveryDocument doc = (AddonDiscoveryDocument)DocumentManager.LoadDocument(typeof(AddonDiscoveryDocument));

            if (doc == null)
                MessageManager.Send(MessageSeverity.Alert,
                    string.Format(Message_en.DocumentCreationError, FileName));

            return doc;
        }

        public void ToXml()
        {
            DocumentManager.ToXml(this);
        }

    }
}
