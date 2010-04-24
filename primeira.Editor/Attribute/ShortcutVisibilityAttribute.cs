using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public sealed class ShortcutVisibilityAttribute : Attribute
    {
        public string Escope { get; set; }

        public Keys DefaultKey{ get; set; }

        public KeyModifiers DefaultKeyModifiers{ get; set; }

        public string Name{ get; set; }

        public string Description{ get; set; }

        public KeyEvent Event { get; set; }

        public ShortcutVisibilityAttribute()
        {
            if (Escope == string.Empty)
                Escope = BasicEscopes.Global;

            if (Event == 0)
                Event = KeyEvent.HotKey;

        }

        public ShortcutVisibilityAttribute(string name, string description, string escope, Keys defaultKey)
            :this()
        {
            Name = name;
            Description = description;
            DefaultKey = defaultKey;
            Escope = escope;
            
        }

        public ShortcutVisibilityAttribute(string name, string description, string escope, Keys defaultKey, KeyModifiers defaultKeyModifiers)
            : this(name, description, escope, defaultKey)
        {
            DefaultKeyModifiers = defaultKeyModifiers;
        }
    }
}
