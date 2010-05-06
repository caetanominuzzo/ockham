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
        internal static class NativeMethods
        {
            [DllImport("dwmapi.dll")]
            internal static extern int DwmExtendFrameIntoClientArea(
               IntPtr hWnd,
               ref MARGINS pMarInset
            );

            [StructLayout(LayoutKind.Sequential)]
            internal struct MARGINS
            {
                public int cxLeftWidth;
                public int cxRightWidth;
                public int cyTopHeight;
                public int cyBottomHeight;
            }

        }

        internal static void SeventishIt(Control control)
        {
            control.BackColor = System.Drawing.Color.Black;

            NativeMethods.MARGINS margins = new NativeMethods.MARGINS();
            margins.cxLeftWidth = 0;
            margins.cxRightWidth = 0;
            margins.cyTopHeight = 33;
            margins.cyBottomHeight = 0;

            IntPtr hWnd = control.Handle;
            NativeMethods.DwmExtendFrameIntoClientArea(hWnd, ref margins);
        }
    }
}
