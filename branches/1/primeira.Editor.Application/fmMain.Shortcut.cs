﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class fmMain
    {
        [ShortcutVisibility("Nome", "", BasicEscopes.Global, Keys.A, KeyModifiers.Alt)]
        public static void show()
        {
            ShortcutManager.ShowConfig();
        }


    }
}