using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace primeira.Editor
{
    public class AddonManager
    {
        private static List<Type> _addonTypes;

        public static void Discovery()
        {
            _addonTypes = new List<Type>();

            string addonDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");

            if (!Directory.Exists(addonDir))
                Directory.CreateDirectory(addonDir);

            string[] dlls = Directory.GetFiles(addonDir, "*.dll", SearchOption.AllDirectories);

            Assembly ass = null;

            List<Type> addons = new List<Type>();

            foreach (string dll in dlls)
            {
                ass = Assembly.LoadFile(dll);

                Type[] types = ass.GetTypes();

                foreach (Type type in types)
                {
                    if (type.GetInterface("IAddon") != null)
                        addons.Add(type);
                }
            }

            InitializeAddonGroup(ref addons, AddonDefinitions.SystemAddon);

            InitializeAddonGroup(ref addons, AddonDefinitions.UserAddon);

            if (EditorContainerManager.IsInitialized())
                InitializeAddonGroup(ref addons, AddonDefinitions.WaitEditorContainer);

            InitializeAddonGroup(ref addons, AddonDefinitions.SystemDelayedInitializationAddon);

            if (addons.Count > 0)
                InitializationError(addons);

        }

        private static void InitializeAddonGroup(ref List<Type> addons, AddonDefinitions definitionFilter)
        {
            InitializeAddons(ref addons, definitionFilter);

            int iLastPendencies = 0, iPendencies = 0;

            iPendencies = addons.Count;

            while (iPendencies > 0 && iLastPendencies != iPendencies)
            {
                iLastPendencies = addons.Count;

                InitializeAddons(ref addons, definitionFilter);

                iPendencies = addons.Count;
            }
        }

        private static void InitializeAddons(ref List<Type> addons, AddonDefinitions definitionsFilter)
        {
            List<Type> pendencies = new List<Type>();

            foreach (Type addon in addons)
            {
                AddonDefinitionAttribute[] attr = (AddonDefinitionAttribute[])addon.GetCustomAttributes(typeof(AddonDefinitionAttribute), false);

                if (attr.Length > 0)
                    if ((attr[0].Definition & definitionsFilter) == 0)
                    {
                        pendencies.Add(addon);
                        continue;
                    }

                try
                {
                    InitializeAddon(addon);
                }
                catch (AddonDependencyException)
                {
                    pendencies.Add(addon);
                }
            }

            addons = pendencies;
        }

        private static void InitializeAddon(Type type)
        {
            try
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    if (method.GetCustomAttributes(typeof(AddonInitializeAttribute), false).Length > 0)
                    {
                        method.Invoke(null, new object[] { });
                        break;
                    }
                }

                _addonTypes.Add(type);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is AddonDependencyException)
                    throw ex.InnerException;
            }
        }

        private static void InitializationError(List<Type> addons)
        {

            StringBuilder sb = new StringBuilder(); 

            foreach (Type error in addons)
            {
                try
                {
                    InitializeAddon(error);
                }
                catch (AddonDependencyException ex)
                {
                    sb.AppendFormat("Addon '{0}' failed to initialize due dependency with '{1}'.", error.Name, ex.Dependency);
                }
            }

            throw new AddonDependencyException(sb.ToString());
        }
    }
}
