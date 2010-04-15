using System;
using System.Collections.Generic;
using System.Text;

namespace primeira.Editor
{
    public interface IShorcutEscopeProvider
    {
        bool IsAtiveByEscope(string escope);
    }
}
