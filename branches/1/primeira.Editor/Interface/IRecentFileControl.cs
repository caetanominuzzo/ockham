﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public interface IRecentFileControl
    {
        void AddRecent(string fileName);

        string[] GetRecent();
    }
}
