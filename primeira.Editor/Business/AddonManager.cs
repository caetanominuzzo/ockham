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

        private static string ADDON_DIR = "Addons";

        /// <summary>
        /// Discovers and initializes addons.
        /// </summary>
        public static void Discovery()
        {
            try
            {
                _addonTypes = new List<Type>();

                List<Type> addons = new List<Type>();

                string addonDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ADDON_DIR);

                if (!Directory.Exists(addonDir))
                    Directory.CreateDirectory(addonDir);

                GetAllAvaiableAddonTypes(ref addons, addonDir);

                AddonDiscoveryDocument cache = AddonDiscoveryDocument.GetInstance();

                string addonCacheFile = AddonDiscoveryDocument.FileName;

                if (cache.LoadOrder.Length > 0 && Directory.GetLastWriteTime(addonDir) <= File.GetLastWriteTime(addonCacheFile))
                {
                    InitializeAddonsFromCache(ref addons, cache);
                }
                else
                {
                    cache.Clear();

                    InitializeAddonGroupByGroup(ref addons, cache);

                    cache.ToXml();

                    if (addons.Count > 0)
                        InitializationError(addons);
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Log(Message_en.AddonDiscoveryError, Environment.NewLine, ex.ToString());
                throw;
            }
        }

        private static void GetAllAvaiableAddonTypes(ref List<Type> addons, string addonDir)
        {
            
            string[] dlls = Directory.GetFiles(addonDir, "*.dll", SearchOption.AllDirectories);
            //string[] dlls = Directory.GetDirectories(addonDir);

            Assembly ass = null;

            foreach (string dll in dlls)
            {
                //ass = Assembly.LoadFrom(@"D:\Desenv\Ockham\branches\1\primeira.Editor.Application\bin\Debug\Addons\TabControlEditor\primeira.Editor.TabControlEditor.dll");
                ass = Assembly.LoadFrom(dll);

                Type[] types = ass.GetExportedTypes();

                foreach (Type type in types)
                {
                    if (type.GetCustomAttributes(typeof(AddonDefinitionAttribute), true).Length > 0)
                        addons.Add(type);
                }

             //   break;
            }
        }

        private static void InitializeAddonsFromCache(ref List<Type> addons, AddonDiscoveryDocument cache)
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

        private static void InitializeAddonGroupByGroup(ref List<Type> addons, AddonDiscoveryDocument cache)
        {
            InitializeAddonGroup(ref addons, AddonOptions.SystemAddon, cache);

            InitializeAddonGroup(ref addons, AddonOptions.SystemDelayedInitializationAddon, cache);

            InitializeAddonGroup(ref addons, AddonOptions.UserAddon, cache);

            InitializeAddonGroup(ref addons, AddonOptions.LastInitilizedAddon, cache);
        }

        private static void InitializeAddonGroup(ref List<Type> addons, AddonOptions definitionFilter, AddonDiscoveryDocument cache)
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

        private static void InitializeAddons(ref List<Type> addons, AddonOptions definitionsFilter, AddonDiscoveryDocument cache)
        {
            List<Type> pendencies = new List<Type>();

            foreach (Type addon in addons)
            {
                AddonDefinitionAttribute[] attr = (AddonDefinitionAttribute[])addon.GetCustomAttributes(typeof(AddonDefinitionAttribute), false);

                if (attr.Length > 0)
                    if ((attr[0].Options & definitionsFilter) == 0)
                    {
                        pendencies.Add(addon);
                        continue;
                    }

                try
                {
                    InitializeAddon(addon, cache);
                }
                catch 
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
                catch
                {
                    sb.AppendFormat("Addon '{0}' failed to initialize.");
                }
            }

            throw new InvalidOperationException(sb.ToString());
        }
    }
}
