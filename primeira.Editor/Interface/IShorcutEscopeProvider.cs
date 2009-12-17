using System;
using System.Collections.Generic;
using System.Text;

namespace primeira.Editor
{
    public interface IShorcutEscopeProvider
    {
        bool IsAtiveByControl(string controlName);

        bool IsAtiveByEscope(string escope);
    }
}
