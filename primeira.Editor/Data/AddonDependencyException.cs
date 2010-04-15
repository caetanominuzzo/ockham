using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primeira.Editor
{
    public class AddonDependencyException : Exception
    {
        public string Dependency { get; private set; }

        public AddonDependencyException(string dependency)
        {
            Dependency = dependency;
        }

    }
}
