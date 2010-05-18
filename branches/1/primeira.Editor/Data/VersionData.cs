using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace primeira.Editor
{
    [DataContract()]
    public class VersionData
    {
        [DataMember()]
        public Guid Id { get; set; }

        [DataMember()]
        public string Number { get; set; }

        [DataMember()]
        public Authorship Author { get; set; }

        public VersionData(string id, string number, string author, string info, string email, string website)
        {
            if(id != null)
                this.Id = new Guid(id);

            this.Number = number;

            this.Author = new Authorship();

            this.Author.Name = author;

            this.Author.Info = info;

            this.Author.Email = email;

            this.Author.Website = website;
        }
    }
}
