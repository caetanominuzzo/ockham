using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public interface IRevision
    {
        void ToXml(string filename);

        string ParentRevision { get; set;  }

        string[] ChildRevision { get; set;  }
    }
}
