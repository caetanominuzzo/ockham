using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor.Business
{
    public static class MessageManager
    {

        public delegate void OnAlertDelegate(string message);
        public static event OnAlertDelegate OnAlert;

        public static void Alert(params string[] message)
        {
            if (OnAlert != null)
                OnAlert(string.Join(string.Empty, message));
        }

    }
}
