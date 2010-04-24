using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public interface IMessageControl
    {
        void Send(MessageSeverity severity, string message);
    }
}
