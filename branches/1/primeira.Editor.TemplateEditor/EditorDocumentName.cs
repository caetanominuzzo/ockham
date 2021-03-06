﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using primeira.Editor.Business;

namespace primeira.Editor.EditorTemplate
{
    [DataContract()]
    public class EditorDocumentName : DocumentBase
    {
        private static DocumentDefinition _definition =
            new DocumentDefinition()
            {
                Name = "Template",
                DefaultName = "template",
                Description = "Template",
                Extension = ".temp",
                Id = new Guid("513ff96c-0d23-44f4-82ab-0dea5a62dcd3"),
                DefaultEditor = typeof(EditorName),
                Options = DocumentDefinitionOptions.UserFile,
                Icon = Image.FromFile(@"D:\Desenv\Ockham\branches\1\primeira.Editor.TemplateEditor\Resources\new.bmp", true)
            };

        public static DocumentDefinition DocumentDefinition
        {
            get { return _definition; }
        }

        public override DocumentDefinition GetDefinition
        {
            get { return _definition; }
        }

        [DataMember()]
        public object Data { get; set; }

        public static DocumentBase ToObject(string filename)
        {
            return DocumentBase.ToObject(filename, typeof(EditorDocumentName));
        }
    }
}


