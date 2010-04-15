using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public class MessageManager
    {
        private static IMessageControl _messageControl;

        private delegate void OnAlertDelegate(string message);

        private static event OnAlertDelegate OnAlert;

        public static void Alert(params string[] message)
        {
            if (OnAlert != null)
                OnAlert(string.Join(string.Empty, message));
        }

        public static void SetMessageControl(IMessageControl messageControl)
        {
            _messageControl = messageControl;
            OnAlert += new OnAlertDelegate(messageControl.ShowNonModalMessage);
        }

        public static bool IsInitialized()
        {
            return _messageControl != null;
        }
    }
}
