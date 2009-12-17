using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace primeira.Editor
{
    internal static class DwmHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(
           IntPtr hWnd,
           ref MARGINS pMarInset
        );


        internal static void SeventishIt(Control control)
        {
            MARGINS margins = new MARGINS();
            margins.cxLeftWidth = 0;
            margins.cxRightWidth = 0;
            margins.cyTopHeight = 28;
            margins.cyBottomHeight = 0;

            IntPtr hWnd = control.Handle;
            int result = DwmExtendFrameIntoClientArea(hWnd, ref margins);

        }


    }
}
