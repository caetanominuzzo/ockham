using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using Shortcut = primeira.Editor;
using System.Reflection;

namespace primeira.Editor
{
    public class ShortcutManager : IMessageFilter
    {

        #region Native win32 API

        private const int WM_HOTKEY = 0x0312;

        private static IntPtr _parentHandle = (IntPtr)0;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GlobalAddAtom(string LPCTSTR);

        #endregion

        private static List<Shortcut> _shortcuts = new List<Shortcut>();

        private static List<ShortcutCommand> _commands = new List<ShortcutCommand>();

        public static IShorcutEscopeProvider EscopeProvider { get; set; }

        internal static ShortcutCommand[] Commands
        {
            get { return _commands.ToArray(); }
        }

        internal static Shortcut[] Shorcuts
        {
            get { return _shortcuts.ToArray(); }
        }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    foreach (Shortcut p in _shortcuts)
                    {
                        if (p.AtomID == (int)m.WParam && EscopeProvider != null && EscopeProvider.IsAtiveByEscope(p.Escope))
                        {
                            p.Command.Method.Invoke(p.Command.Object, new object[0]);
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }


        internal static ShortcutCommand AddCommand(string name, string description, MethodInfo method, Object aObject, string escope)
        {
            ShortcutCommand c = new ShortcutCommand();

            c.Name = name;
            c.Description = description;
            c.Method = method;
            c.Object = aObject;
            c.Escope = escope;
            _commands.Add(c);

            return c;
        }

        internal static void AddShortcut(string escope, Keys key, KeyModifiers keyModifiers, ShortcutCommand command)
        {
            Shortcut s = new Shortcut();

            s.Key = key;
            s.KeyModifier = keyModifiers;
            s.AtomID = GlobalAddAtom(s.ToString());
            s.Escope = escope;
            s.Command = command;
            _shortcuts.Add(s);

            s.Register(_parentHandle);
        }

        public static void LoadFromForm(Form aForm)
        {
            Application.AddMessageFilter(new ShortcutManager());
            _parentHandle = aForm.Handle;
            foreach (MethodInfo m in aForm.GetType().GetMethods())
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
                            command = AddCommand(v.Name, v.Description, m, aForm, v.Escope);

                        AddShortcut(v.Escope, v.DefaultKey, v.DefaultKeyModifiers, command);
                    }
            }

        }

        public void Assign(string aName, string aEscope, Keys aKey, KeyModifiers aKeyModifiers)
        {
            Shortcut shortcut = new Shortcut();

            foreach (Shortcut p in _shortcuts)
            {
                if (p.Key == aKey && p.KeyModifier == aKeyModifiers && p.Escope == aEscope)
                {
                    Unassign(p);
                    shortcut = p;
                    break;
                }
            }

            ShortcutCommand command = new ShortcutCommand();

            foreach (ShortcutCommand p in _commands)
            {
                if (p.Name == aName)
                {
                    command = p;
                    break;

                }
            }

            AddShortcut(aEscope, aKey, aKeyModifiers, command);

        }

        internal void Unassign(Shortcut aShorcut)
        {

            _shortcuts.Remove(aShorcut);

            //Verify if another command is using that atomID.
            //If not unregister key
            foreach (Shortcut p in _shortcuts)
            {
                if (p.AtomID == aShorcut.AtomID)
                    return;
            }

            UnregisterHotKey(_parentHandle, aShorcut.AtomID);

        }

        public static void ShowConfig()
        {
            EditorManager.LoadEditor("default.shortcut");
         //   fmShortcutConfig p = new fmShortcutConfig(this);
        //    p.ShowDialog();
        }

    }
}
