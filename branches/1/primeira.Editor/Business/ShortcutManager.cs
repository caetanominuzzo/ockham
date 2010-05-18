using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class ShortcutManager  : IMessageFilter
    {
        #region PreFilter

        private static ShortcutManager _preFilter;

        public bool PreFilterMessage(ref Message m)
        {
            if (_active &&
                (m.Msg == (int)KeyEvent.KeyUp ||
                m.Msg == (int)KeyEvent.KeyDown))
            {
                Keys control = Control.ModifierKeys;

                foreach (Shortcut p in _shortcuts)
                {
                    if (
                        p.Target.Count > 0
                        && (int)p.Event == m.Msg
                        && ((int)p.Key == (int)m.WParam)
                        && VerifyKeys(p.Modifiers, control))
                    {
                        foreach (object target in p.Target)
                        {
                            if(VerifyEscope((Control)target, p.Escope))
                                p.Method.Invoke(target, new object[0]);
                        }
                    }
                }
            }
            return false;
        }

        private bool VerifyKeys(Keys modifiers, Keys current)
        {
            bool AllKeysPressed = true;

            if (modifiers.HasFlag(Keys.Alt))
                AllKeysPressed = current.HasFlag(Keys.Alt);

            if (AllKeysPressed && modifiers.HasFlag(Keys.Control))
                AllKeysPressed = current.HasFlag(Keys.Control);

            if (AllKeysPressed && modifiers.HasFlag(Keys.Shift))
                AllKeysPressed = current.HasFlag(Keys.Shift);

            return AllKeysPressed;
        }

        private bool VerifyEscope(Control control, string escope)
        {
            if (escope == BasicEscopes.Global)
                return true;

            if (control == null)
                return false;

            if (control is ContainerControl)
            {
                ContainerControl container = (ContainerControl)control;

                if (control.GetType().GetInterfaces().Contains(typeof(IShorcutEscopeProvider)))
                    if (((IShorcutEscopeProvider)control).IsAtiveByEscope(escope))
                        return true;
            }
            return false;
        }

        #endregion

        #region Shortcut Editor

        public static void ShowConfig()
        {
            DocumentHeader header = DocumentManager.RegisterDocument(typeof(ShortcutConfigDocument));

            EditorManager.LoadEditor(header);
        }

        #endregion

        #region fields

        private static List<Shortcut> _shortcuts = new List<Shortcut>();

        private static bool _active = true;

        #endregion

        #region Properties

        public static Shortcut[] Shorcuts
        {
            get { return _shortcuts.ToArray(); }
        }

        #endregion

        #region Load & Assign shortcuts

        private ShortcutManager()
        { }

        public static void InitializePreFilter()
        {
            if (_preFilter == null)
            {
                _preFilter = new ShortcutManager();

                Application.AddMessageFilter(_preFilter);
            }
        }

        public static void PausePreFilter()
        {
            _active = false;
        }

        public static void ResumePreFilter()
        {
            _active = true;
        }

        private static Shortcut AddShortcut(
            string name, string description, MethodInfo method, Object target,
            string escope, Keys key, Keys Keys)
        {
            return AddShortcut(name, description, method, target, escope, key, Keys, KeyEvent.KeyUp);
        }

        private static Shortcut AddShortcut(
            string name, string description, MethodInfo method, Object target,
            string escope, Keys key, Keys Keys, KeyEvent keyEvent)
        {
            //if (name.IndexOf("Close Tab") == -1)
            //    return null;

            Shortcut s = null;

            if (target == null)
            {
                s = new Shortcut();

                s.Name = name;
                s.Description = description;
                s.Method = method;
                s.Escope = escope;
                s.Key = key;
                s.Modifiers = Keys;
                s.Event = keyEvent;

                _shortcuts.Add(s);
            }
            else
            {
                s = _shortcuts.Find(x => x.Method == method && x.Key == key && x.Modifiers == Keys);

                if (s == null)
                {
                    LoadFromType(target.GetType(), null);

                    s = _shortcuts.Find(x => x.Method == method && x.Key == key && x.Modifiers == Keys);
                }
                
                s.Target.Add(target);
            }
            

            return s;
        }

        private static Keys _validKeyModifiers = Keys.Control | Keys.Shift | Keys.Alt;

        public static void LoadFromForm(Control control)
        {
            LoadFromType(control.GetType(), control);
        }

        public static void LoadFromType(Type type)
        {
            LoadFromType(type, null);
        }

        private static void LoadFromType(Type type, Control control)
        {
            foreach (MethodInfo m in type.GetMethods())
            {
                foreach (object o in m.GetCustomAttributes(false))
                    if (o is ShortcutVisibilityAttribute)
                    {
                        ShortcutVisibilityAttribute v = (ShortcutVisibilityAttribute)o;

                        if (!_validKeyModifiers.HasFlag(v.DefaultKeys))
                        {
                            LogFileManager.Log(Message_en.ShortcutKeyModifierMustBeAltControlOrShift);

                            throw new InvalidOperationException(Message_en.ShortcutKeyModifierMustBeAltControlOrShift);
                        }

                        AddShortcut(v.Name, v.Description, m, control, v.Escope, v.DefaultKey, v.DefaultKeys, v.Event);
                    }
            }
        }

        public static void Assign(Shortcut shortcut, string escope, Keys key, Keys Keys)
        {
            Shortcut s = shortcut.New(escope, key, Keys);
            _shortcuts.Add(s);
        }

        public static void Unassign(Shortcut shortcut)
        {
            shortcut.Key = Keys.None;
            shortcut.Modifiers = Keys.None;
        }

        public static void Remove(Shortcut shortcut)
        {
            _shortcuts.Remove(shortcut);
        }
        

        #endregion

    }
}
