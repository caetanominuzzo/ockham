using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class ShortCutVisibilityAttribute : Attribute
    {
        private string _name;
        private string _description;
        private Keys _defaultKey;
        private KeyModifiers _defaultKeyModifiers;
        private string _escope;

        public string Escope
        {
            get { return _escope; }
            set { _escope = value; }
        }


        public Keys DefaultKey
        {
            get { return _defaultKey; }
            set { _defaultKey = value; }
        }

        public KeyModifiers DefaultKeyModifiers
        {
            get { return _defaultKeyModifiers; }
            set { _defaultKeyModifiers = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ShortCutVisibilityAttribute(string name, string description, string escope, Keys defaultKey)
        {
            Name = name;
            Description = description;
            DefaultKey = defaultKey;
            Escope = escope;
        }

        public ShortCutVisibilityAttribute(string name, string description, string escope, Keys defaultKey, KeyModifiers defaultKeyModifiers)
            : this(name, description, escope, defaultKey)
        {
            DefaultKeyModifiers = defaultKeyModifiers;
        }
    }
}
