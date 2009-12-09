using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public interface ITabControl
    {
        void HideTab(IEditorBase tab);

        void CloseHidedTabs();

        void AddTab(IEditorBase tab);

        ITabButton CreateTabButton();
    }
}
