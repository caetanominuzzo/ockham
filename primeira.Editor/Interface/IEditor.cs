using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace primeira.Editor
{
    
    public interface IEditor
    {
        ITabButton TabButton { get; }

        DocumentBase Document { get; }

        string Filename { get; }

        bool Selected { get; set;  }

        bool ShowCloseButton { get; }

        event SelectedDelegate OnSelected;

    }

}
