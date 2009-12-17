using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public interface ITabControl
    {
        void HideTab(IEditor tab);

        void CloseHideTabs();

        void AddTab(IEditor tab);

        ITabButton CreateTabButton(IEditor editor);
    }
}
