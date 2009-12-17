using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace primeira.Editor
{
    public interface ITabButton
    {
        string TabTitle { get; }

        void Invalidate();

        event EventHandler Click;

        void SetText(string tooltip);

        void SetWidth(int width);

        
    }
}
