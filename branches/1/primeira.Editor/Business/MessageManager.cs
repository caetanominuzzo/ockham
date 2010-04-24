using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public class MessageManager
    {
        private static IMessageControl _messageControl;

        private delegate void OnMessageDelegate(MessageSeverity severity, string message);

        private static event OnMessageDelegate OnMessage;

        public static void Send(params string[] message)
        {
            Send(MessageSeverity.Information, message);
        }

        public static void Send(MessageSeverity severity, params string[] message)
        {
            if (OnMessage != null)
                OnMessage(severity, string.Join(string.Empty, message));
        }

        public static void SetMessageControl(IMessageControl messageControl)
        {
            _messageControl = messageControl;
            OnMessage += new OnMessageDelegate(messageControl.Send);
        }

        public static bool IsInitialized()
        {
            return _messageControl != null;
        }
    }
}
