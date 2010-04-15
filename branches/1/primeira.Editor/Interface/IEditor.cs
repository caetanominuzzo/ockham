using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace primeira.Editor
{
    
    public interface IEditor
    {
        DocumentBase Document { get; }

        string Filename { get; }

        bool Selected { get; set;  }

        event SelectedDelegate OnSelected;

        bool HasOption(DocumentDefinitionOptions Option);
    }

}
