﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace primeira.Editor.Business
{
    public delegate void SelectedDelegate(IEditorBase sender);

    public interface IEditorBase
    {
        ITabButton TabButton { get; }

        DocumentBase Document { get; }

        string Filename { get; }

        bool Selected { get; set;  }

        bool ShowCloseButton { get; }

        event SelectedDelegate OnSelected;

        DateTime TimeOPen { get; }

    }

}
