﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public interface IMessageControl
    {
        void ShowNonModalMessage(string message);
    }
}