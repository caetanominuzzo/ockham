using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

namespace primeira.Editor
{
    public class ShortcutManager : IMessageFilter
    {

        #region Native win32 API

        private static IntPtr _parentHandle = (IntPtr)0;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GlobalAddAtom(string LPCTSTR);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

         
        const int VK_LSHIFT = 0xA0;
        const int VK_RSHIFT = 0xA1;
        const int VK_LCONTROL = 0xA2;
        const int VK_RCONTROL = 0xA3;
        const int VK_LMENU = 0xA4;
        const int VK_RMENU = 0x5A;


        #endregion

        private static Type _shortcutConfigDocumentType;

        private static List<Shortcut> _shortcuts = new List<Shortcut>();
        private static List<Shortcut> _shortcutsUp = new List<Shortcut>();
        private static List<Shortcut> _shortcutsDown = new List<Shortcut>();

        private static List<ShortcutCommand> _commands = new List<ShortcutCommand>();

        public static ShortcutCommand[] Commands
        {
            get { return _commands.ToArray(); }
        }

        public static Shortcut[] Shorcuts
        {
            get { return _shortcuts.ToArray(); }
        }

        public static Control ParentEscopeProvider { get; set; }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)KeyEvent.HotKey:
                    foreach (Shortcut p in _shortcuts)
                    {
                        if (p.AtomID == (int)m.WParam && VerifyEscope(ParentEscopeProvider, p.Escope))
                        {
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                            return true;
                        }
                    }
                    break;
                case (int)KeyEvent.KeyUp:
                    foreach (Shortcut p in _shortcutsUp)
                    {
                        if ((int)p.Key == (int)m.WParam && VerifyEscope(ParentEscopeProvider, p.Escope))
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                    }
                    break;
                case (int)KeyEvent.KeyDown:
                    foreach (Shortcut p in _shortcutsDown)
                    {
                        if ((int)p.Key == (int)m.WParam && VerifyEscope(ParentEscopeProvider, p.Escope))
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                    }
                    break; 

            }
            return false;
        }

        public delegate void CallbackDelegate();

        private static Dictionary<int, CallbackDelegate> _keyUpCallback = new Dictionary<int, CallbackDelegate>();

        public static void RegisterCallbackForKeyUp(int key, CallbackDelegate callback)
        {
            _keyUpCallback.Add(key, callback);
        }

        public static bool KeyPressed(Keys key)
        {
            return (GetKeyState(VK_LCONTROL) > 0 || GetKeyState(VK_RCONTROL) > 0);
                
        }

        public static bool VerifyEscope(Control control, string escope)
        {
            if (escope == BasicEscopes.Global)
                return true;

            if (control is ContainerControl)
            {
                ContainerControl container = (ContainerControl)control;

                if (container == null)
                    return false;

                if (control.GetType().GetInterfaces().Contains(typeof(IShorcutEscopeProvider)))
                    if (((IShorcutEscopeProvider)control).IsAtiveByEscope(escope))
                        return true;

                return VerifyEscope(container.ActiveControl, escope);
            }
            return false;
        }

        internal static ShortcutCommand AddCommand(string name, string description, MethodInfo method, Object target, string escope)
        {
            ShortcutCommand c = new ShortcutCommand();

            c.Name = name;
            c.Description = description;
            c.Method = method;
            c.Target = target;
            c.Escope = escope;
            _commands.Add(c);

            return c;
        }

        internal static Shortcut AddShortcut(string escope, Keys key, KeyModifiers keyModifiers, ShortcutCommand command)
        {
            return AddShortcut(escope, key, keyModifiers, KeyEvent.HotKey, command);
        }

        internal static Shortcut AddShortcut(string escope, Keys key, KeyModifiers keyModifiers, KeyEvent keyEvent, ShortcutCommand command)
        {
            Shortcut s = new Shortcut();

            s.Key = key;
            s.KeyModifier = keyModifiers;
            s.AtomID = GlobalAddAtom(s.ToString());
            s.Escope = escope;
            s.Command = command;
            s.Event = keyEvent;

            if (keyEvent == KeyEvent.HotKey)
                _shortcuts.Add(s);
            else if (keyEvent == KeyEvent.KeyUp)
                _shortcutsUp.Add(s);
            else if (keyEvent == KeyEvent.KeyDown)
                _shortcutsDown.Add(s);

            s.Register(_parentHandle);

            return s;
        }

        static System.Text.StringBuilder sb = new System.Text.StringBuilder();

        public static void LoadFromForm(Control control)
        {
            Application.AddMessageFilter(new ShortcutManager());
            _parentHandle = control.Handle;

            foreach (MethodInfo m in control.GetType().GetMethods())
            {
                foreach (object o in m.GetCustomAttributes(false))
                    if (o is ShortcutVisibilityAttribute)
                    {
                        ShortcutVisibilityAttribute v = (ShortcutVisibilityAttribute)o;

                        ShortcutCommand command = null;

                        foreach (ShortcutCommand cc in _commands)
                        {
                            if (cc.Name == v.Name)
                            {
                                command = cc;
                                break;
                            }
                        }

                        if (command == null)
                            command = AddCommand(v.Name, v.Description, m, control, v.Escope);

                        sb.Append(control.GetType().FullName + m.Name + "\n");

                        AddShortcut(v.Escope, v.DefaultKey, v.DefaultKeyModifiers, v.Event, command);
                    }
            }

        }

        public static void Assign(string name, string escope, Keys key, KeyModifiers keyModifiers)
        {
            Shortcut shortcut = new Shortcut();

            foreach (Shortcut p in _shortcuts)
            {
                if (p.Key == key && p.KeyModifier == keyModifiers && p.Escope == escope)
                {
                    Unassign(p);
                    shortcut = p;
                    break;
                }
            }

            ShortcutCommand command = new ShortcutCommand();

            foreach (ShortcutCommand p in _commands)
            {
                if (p.Name == name)
                {
                    command = p;
                    break;

                }
            }

            AddShortcut(escope, key, keyModifiers, command);

        }

        public static void Unassign(Shortcut shorcut)
        {
            _shortcuts.Remove(shorcut);

            //Verify if another command is using that atomID.
            //If not unregister key
            foreach (Shortcut p in _shortcuts)
            {
                if (p.AtomID == shorcut.AtomID)
                    return;
            }

            UnregisterHotKey(_parentHandle, shorcut.AtomID);

        }

        public static void SetShortcutConfigDocumentType(Type editor)
        {
            _shortcutConfigDocumentType = editor;
        }

        public static void ShowConfig()
        {
            EditorManager.LoadEditor(_shortcutConfigDocumentType);
        }

    }
}
