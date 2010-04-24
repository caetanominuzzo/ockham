using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public static class Message_us
    {
        public static string TabControlAlreadySet                               = "You can't change tab control after it has been changed already.";
        public static string DocumentCustomSerializationMustHaveToObjectMethod  = "Documents with CustomSerialization option must implement a ToObject(string) method.";
        public static string DocumentCustomSerializatinoFail                     = "There is a problem with the custom serialization in '{0}' addon. Try getting a new version.";
    }
}
