using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public class MessageManager
    {
        private delegate void OnAlertDelegate(string message);
        private static event OnAlertDelegate OnAlert;

        public static void Alert(params string[] message)
        {
            if (OnAlert != null)
                OnAlert(string.Join(string.Empty, message));
        }

        public static void SetMessageControl(IMessageControl messageControl)
        {
            OnAlert += new OnAlertDelegate(messageControl.ShowNonModalMessage);
        }
    }
}
