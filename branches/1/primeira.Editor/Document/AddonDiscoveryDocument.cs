﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;

namespace primeira.Editor
{
    [DataContract()]
    [DocumentHeader(
                Id="F6A86949-950C-448C-9183-90DB3E3651D5",
                VersionNumber = "1.0",
                Name = "Addon Discovery Cache",
                DefaultFileName = "addons",
                Description = "Stores the last sucessful addons discovery.",
                DefaultFileExtension = ".cache",
                Options=DocumentHeaderOptions.OpenFromTypeDefaultName)]
    public class AddonDiscoveryDocument : DocumentBase
    {
        public static string FileName
        {
            get { return "addons.cache"; }
        }

        private List<AddonHeader> _addons = new List<AddonHeader>();

        public void AddHeader(AddonHeader header)
        {
            _addons.Add(header);
        }

        public void Clear()
        {
            _addons.Clear();
        }

        [DataMember()]
        public AddonHeader[] Addons
        {
            get { return _addons.ToArray(); }
            set
            {
                if (_addons == null)
                    _addons = new List<AddonHeader>(value.Length);
                else
                    _addons.Clear();

                _addons.AddRange(value);
            }
        }

        public DateTime LastWriteTime { get; private set; }

        public static AddonDiscoveryDocument GetInstance()
        {
            DocumentHeader header = DocumentManager.RegisterDocument(typeof(AddonDiscoveryDocument));

            AddonDiscoveryDocument doc = (AddonDiscoveryDocument)DocumentManager.LoadDocument(header, FileName);

            FileInfo f = new FileInfo(FileName);

            if (f.Exists)
                doc.LastWriteTime = f.LastWriteTime;

            if (doc == null)
                throw new InvalidOperationException(
                    string.Format(Message_en.DocumentCreationError, FileName));

            return doc;
        }

        public void ToXml()
        {
            DocumentManager.SaveDocument(this, FileName);
        }
    }
}
