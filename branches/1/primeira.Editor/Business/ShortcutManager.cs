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

        private Control _parentEscopeProvider;

        private static ShortcutManager _shortcutManager = null;

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)KeyEvent.KeyUp:
                    foreach (Shortcut p in _shortcutsUp)
                    {
                        if (((int)p.Key == (int)m.WParam)
                            && VerifyKeyModifiers(p.KeyModifier)
                            && VerifyEscope(_parentEscopeProvider, p.Escope))
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                    }
                    break;

                case (int)KeyEvent.KeyDown:
                    foreach (Shortcut p in _shortcutsDown)
                    {
                        if ((int)p.Key == (int)m.WParam && VerifyEscope(_parentEscopeProvider, p.Escope))
                            p.Command.Method.Invoke(p.Command.Target, new object[0]);
                    }
                    break;
            }

            return false;
        }

        private bool VerifyKeyModifiers(KeyModifiers modifiers)
        {
            bool AllKeysPressed = true;

            Keys control = Control.ModifierKeys;

            if (modifiers.HasFlag(KeyModifiers.Alt))
                AllKeysPressed = control.HasFlag(Keys.Alt);

            if (AllKeysPressed && modifiers.HasFlag(KeyModifiers.Control))
                AllKeysPressed = control.HasFlag(Keys.Control);

            if (AllKeysPressed && modifiers.HasFlag(KeyModifiers.Shift))
                AllKeysPressed = control.HasFlag(Keys.Shift);

            return AllKeysPressed;
        }

        private bool VerifyEscope(Control control, string escope)
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

        #region static fields

        private static List<Shortcut> _shortcutsUp = new List<Shortcut>();

        private static List<Shortcut> _shortcutsDown = new List<Shortcut>();

        private static List<ShortcutCommand> _commands = new List<ShortcutCommand>();

        #endregion

        #region Properties

        public static ShortcutCommand[] Commands
        {
            get { return _commands.ToArray(); }
        }

        public static Shortcut[] Shorcuts
        {
            get { return _shortcutsUp.ToArray(); }
        }

        #endregion

        #region Load & Assign shortcuts

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

        private static Shortcut AddShortcut(string escope, Keys key, KeyModifiers keyModifiers, ShortcutCommand command)
        {
            return AddShortcut(escope, key, keyModifiers, KeyEvent.KeyUp, command);
        }

        private static Shortcut AddShortcut(string escope, Keys key, KeyModifiers keyModifiers, KeyEvent keyEvent, ShortcutCommand command)
        {
            Shortcut s = new Shortcut();

            s.Key = key;
            s.KeyModifier = keyModifiers;
            s.Escope = escope;
            s.Command = command;
            s.Event = keyEvent;

            if (keyEvent == KeyEvent.KeyUp)
                _shortcutsUp.Add(s);
            else if (keyEvent == KeyEvent.KeyDown)
                _shortcutsDown.Add(s);

            return s;
        }

        public static void LoadFromForm(Control control)
        {
            if (_shortcutManager == null)
            {
                _shortcutManager = new ShortcutManager();
                _shortcutManager._parentEscopeProvider = Application.OpenForms[0];
                Application.AddMessageFilter(_shortcutManager);
            }
            
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

                        AddShortcut(v.Escope, v.DefaultKey, v.DefaultKeyModifiers, v.Event, command);
                    }
            }

        }

        public static void Assign(string name, string escope, Keys key, KeyModifiers keyModifiers)
        {
            Shortcut shortcut = new Shortcut();

            foreach (Shortcut p in _shortcutsUp)
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
            _shortcutsUp.Remove(shorcut);
        }

        #endregion

    }
}
