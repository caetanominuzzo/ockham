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

        private const int WM_HOTKEY = 0x0312;

        private static IntPtr _parentHandle = (IntPtr)0;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GlobalAddAtom(string LPCTSTR);

        #endregion

        private static Type _shortcutConfigDocumentType;

        private static List<Shortcut> _shortcuts = new List<Shortcut>();

        private static List<ShortcutCommand> _commands = new List<ShortcutCommand>();

        internal static ShortcutCommand[] Commands
        {
            get { return _commands.ToArray(); }
        }

        internal static Shortcut[] Shorcuts
        {
            get { return _shortcuts.ToArray(); }
        }

        public static Control ParentEscopeProvider { get; set; }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    foreach (Shortcut p in _shortcuts)
                    {
                        if (p.AtomID == (int)m.WParam && VerifyEscope(ParentEscopeProvider, p.Escope))
                        {
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public static bool VerifyEscope(Control control, string escope)
        {
            if (escope == BasicEscopes.Global)
                return true;

            var container = (ContainerControl)control;

            if(container == null)
                return false;

            if (control.GetType().GetInterfaces().Contains(typeof(IShorcutEscopeProvider)))
                if (((IShorcutEscopeProvider)control).IsAtiveByEscope(escope))
                    return true;
 
            return VerifyEscope(container.ActiveControl, escope); 
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
            Shortcut s = new Shortcut();

            s.Key = key;
            s.KeyModifier = keyModifiers;
            s.AtomID = GlobalAddAtom(s.ToString());
            s.Escope = escope;
            s.Command = command;
            _shortcuts.Add(s);

            s.Register(_parentHandle);

            return s;
        }

        public static void LoadFromForm(Control control)
        {
            Application.AddMessageFilter(new ShortcutManager());
            _parentHandle = control.Handle;

            foreach (MethodInfo m in control.GetType().GetMethods())
            {
                foreach (object o in m.GetCustomAttributes(false))
                    if (o is ShortCutVisibilityAttribute)
                    {
                        ShortCutVisibilityAttribute v = (ShortCutVisibilityAttribute)o;

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

                        AddShortcut(v.Escope, v.DefaultKey, v.DefaultKeyModifiers, command);
                    }
            }

        }

        public void Assign(string name, string escope, Keys key, KeyModifiers keyModifiers)
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

        internal void Unassign(Shortcut shorcut)
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
