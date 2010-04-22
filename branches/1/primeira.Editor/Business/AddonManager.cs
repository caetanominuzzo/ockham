using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace primeira.Editor
{
    public class AddonManager
    {
        private static List<Type> _addonTypes;

        public static void Discovery()
        {
            _addonTypes = new List<Type>();

            List<Type> addons = new List<Type>();

            string addonDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");

            if (!Directory.Exists(addonDir))
                Directory.CreateDirectory(addonDir);

            string[] dlls = Directory.GetFiles(addonDir, "*.dll", SearchOption.AllDirectories);

            Assembly ass = null;

            foreach (string dll in dlls)
            {
                ass = Assembly.LoadFile(dll);

                Type[] types = ass.GetTypes();

                foreach (Type type in types)
                {
                    if (type.GetCustomAttributes(typeof(AddonDefinitionAttribute), true).Length > 0)
                        addons.Add(type);
                }
            }

            AddonDiscoveryDocument cache = AddonDiscoveryDocument.GetInstance();

            string addonCacheFile = cache.Filename;

            DateTime dtAddonsDir = Directory.GetLastWriteTime(addonDir);

            if (dtAddonsDir <= File.GetLastWriteTime(addonCacheFile))
            {
                AddonDiscoveryDocument.AssemblyTypeDocument[] loadOrder = cache.LoadOrder;

                foreach (AddonDiscoveryDocument.AssemblyTypeDocument assembly in loadOrder)
                {
                    foreach (Type type in addons)
                    {
                        if (type.Assembly.CodeBase.Equals(assembly.AssemblyFile))
                            if (type.Name == assembly.Type)
                                InitializeAddon(type);
                    }
                }
            }
            else
            {
                InitializeAddonGroup(ref addons, AddonDefinitions.SystemAddon, cache);

                InitializeAddonGroup(ref addons, AddonDefinitions.UserAddon, cache);

                if (EditorContainerManager.IsInitialized())
                    InitializeAddonGroup(ref addons, AddonDefinitions.WaitEditorContainer, cache);

                InitializeAddonGroup(ref addons, AddonDefinitions.SystemDelayedInitializationAddon, cache);

                cache.ToXml();

                if (addons.Count > 0)
                    InitializationError(addons);
            }
        }

        private static void InitializeAddonGroup(ref List<Type> addons, AddonDefinitions definitionFilter, AddonDiscoveryDocument cache)
        {
            InitializeAddons(ref addons, definitionFilter, cache);

            int iLastPendencies = 0, iPendencies = 0;

            iPendencies = addons.Count;

            while (iPendencies > 0 && iLastPendencies != iPendencies)
            {
                iLastPendencies = addons.Count;

                InitializeAddons(ref addons, definitionFilter, cache);

                iPendencies = addons.Count;
            }
        }

        private static void InitializeAddons(ref List<Type> addons, AddonDefinitions definitionsFilter, AddonDiscoveryDocument cache)
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
                    InitializeAddon(addon, cache);
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
            InitializeAddon(type, null);
        }

        private static void InitializeAddon(Type type, AddonDiscoveryDocument cache)
        {
            
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    if (method.GetCustomAttributes(typeof(AddonInitializeAttribute), false).Length > 0)
                    {
                        try
                        {
                            method.Invoke(null, new object[] { });
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException is AddonDependencyException)
                                throw ex.InnerException;
                        }

                        break;
                    }
                }

                _addonTypes.Add(type);

                if(cache != null)
                    cache.AddType(type);
            
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
