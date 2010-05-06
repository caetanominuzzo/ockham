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
            if (m.Msg == (int)KeyEvent.KeyUp ||
                m.Msg == (int)KeyEvent.KeyDown)
            {
                foreach (Shortcut p in _shortcuts)
                {
                    if (
                        p.Command.Target != null 
                        && (int)p.Event == m.Msg
                        && ((int)p.Key == (int)m.WParam)
                        && VerifyKeys(p.Modifiers)
                        && VerifyEscope((Control)p.Command.Target, p.Escope))
                    {
                        p.Command.Method.Invoke(p.Command.Target, new object[0]);
                        break;
                    }
                }
            }
            return false;
        }

        private bool VerifyKeys(Keys modifiers)
        {
            bool AllKeysPressed = true;

            Keys control = Control.ModifierKeys;

            if (modifiers.HasFlag(Keys.Alt))
                AllKeysPressed = control.HasFlag(Keys.Alt);

            if (AllKeysPressed && modifiers.HasFlag(Keys.Control))
                AllKeysPressed = control.HasFlag(Keys.Control);

            if (AllKeysPressed && modifiers.HasFlag(Keys.Shift))
                AllKeysPressed = control.HasFlag(Keys.Shift);

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

        private static Type _shortcutConfigDocumentType;

        public static void SetShortcutConfigDocumentType(Type editor)
        {
            _shortcutConfigDocumentType = editor;
        }

        public static void ShowConfig()
        {
            EditorManager.LoadEditor(_shortcutConfigDocumentType);
        }

        #endregion

        #region fields

        private static List<Shortcut> _shortcuts = new List<Shortcut>();

        private static List<ShortcutCommand> _commands = new List<ShortcutCommand>();

        #endregion

        #region Properties

        public static ShortcutCommand[] Commands
        {
            get { return _commands.ToArray(); }
        }

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

        private static ShortcutCommand AddCommand(string name, string description, MethodInfo method, Object target, string escope)
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

        private static Shortcut AddShortcut(string escope, Keys key, Keys Keys, ShortcutCommand command)
        {
            return AddShortcut(escope, key, Keys, KeyEvent.KeyUp, command);
        }

        private static Shortcut AddShortcut(string escope, Keys key, Keys Keys, KeyEvent keyEvent, ShortcutCommand command)
        {
            Shortcut s = new Shortcut();

            s.Key = key;
            s.Modifiers = Keys;
            s.Escope = escope;
            s.Command = command;
            s.Event = keyEvent;

            _shortcuts.Add(s);

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

                        ShortcutCommand command = null;

                        foreach (ShortcutCommand cc in _commands)
                        {
                            if (cc.Name == v.Name && cc.Target == control)
                            {
                                command = cc;
                                break;
                            }
                        }

                        if (command == null)
                            command = AddCommand(v.Name, v.Description, m, control, v.Escope);

                        AddShortcut(v.Escope, v.DefaultKey, v.DefaultKeys, v.Event, command);
                    }
            }
        }

        public static void Assign(string name, string escope, Keys key, Keys Keys)
        {
            Shortcut shortcut = new Shortcut();

            foreach (Shortcut p in _shortcuts)
            {
                if (p.Key == key && p.Modifiers == Keys && p.Escope == escope)
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

            AddShortcut(escope, key, Keys, command);

        }

        public static void Unassign(Shortcut shorcut)
        {
            _shortcuts.Remove(shorcut);
        }

        

        #endregion

    }
}
