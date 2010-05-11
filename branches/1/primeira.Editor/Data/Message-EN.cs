using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public static class Message_en
    {

        public static string TwoTabControlAddons                               = "There is two addons trying to set Tab Control Editor.";
        public static string DocumentCustomSerializationReadMustHaveToObjectMethod  = "Documents with CustomSerializationRead option must implement a ToObject(string) method.";
        public static string DocumentCustomSerializationWriteMustHaveToObjectMethod = "Documents with CustomSerializationWrite option must implement a ToXml(string) method.";
        public static string DocumentCustomSerializatinoError                   = "There is a problem with the custom serialization in '{0}' addon. Try getting a new version.";
        public static string DocumentMissingDocumentHeaderAttribute             = "Missing DocumentHeaderAttribute in '{0}' Class.";
        public static string DocumentMissingOpenFromTypeDefaultName             = "To load a document by the document type it must have the OpenFromTypeDefaultName option.";
        public static string AddonDiscoveryError                                = "There is an error discovering addons.";
        public static string EditorCreationError                                = "There is an error loading editor for file '{0}'.";
        public static string ThereIsNoEditorForType                             = "There is no registered editor for file '{0}'.";
        public static string DocumentCreationError                              = "There is an error loading document '{0}'.";
        public static string ShortcutKeyModifierMustBeAltControlOrShift         = "Shortcut key modifier must be only Alt, Control or Shift key.";
        public static string ShortcutNoConflictsDetected                        = "No conflicts detected.";
    }

}
