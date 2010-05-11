using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace primeira.Editor
{
    public class AddonManager
    {
        private static AddonDiscoveryDocument cache = null;

        private static List<Type> addons = null;

        private static string ADDON_DIR = "Addons";

        /// <summary>
        /// Discovers and initializes all available addons.
        /// 
        /// If there is a addon discovery cache file newer than the addons dir it will be loaded.
        /// </summary>
        public static void Discovery()
        {
            try
            {
                addons = new List<Type>();

                string addonDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ADDON_DIR);

                if (!Directory.Exists(addonDir))
                    Directory.CreateDirectory(addonDir);

                GetAllAvaiableAddonTypes(addonDir);

                cache = AddonDiscoveryDocument.GetInstance();

                string addonCacheFile = AddonDiscoveryDocument.FileName;

                if (cache.LoadOrder.Length > 0 && Directory.GetLastWriteTime(addonDir) <= File.GetLastWriteTime(addonCacheFile))
                {
                    InitializeAddonsFromCache();
                }
                else
                {
                    cache.Clear();

                    InitializeAddonGroupByGroup();

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

        private static void GetAllAvaiableAddonTypes(string addonDir)
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
                    if (type.GetCustomAttributes(typeof(AddonAttribute), true).Length > 0)
                        addons.Add(type);
                }

                //   break;
            }
        }

        private static void InitializeAddonsFromCache()
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

        private static void InitializeAddonGroupByGroup()
        {
            InitializeAddonGroup(AddonOptions.SystemAddon);

            InitializeAddonGroup(AddonOptions.SystemDelayedInitializationAddon);

            InitializeAddonGroup(AddonOptions.UserAddon);

            InitializeAddonGroup(AddonOptions.LastInitilizedAddon);
        }

        private static void InitializeAddonGroup(AddonOptions optionFilter)
        {
            InitializeAddons(optionFilter);

            int iLastPendencies = 0, iPendencies = 0;

            iPendencies = addons.Count;

            while (iPendencies > 0 && iLastPendencies != iPendencies)
            {
                iLastPendencies = addons.Count;

                InitializeAddons(optionFilter);

                iPendencies = addons.Count;
            }
        }

        private static void InitializeAddons(AddonOptions optionFilter)
        {
            List<Type> pendencies = new List<Type>();

            foreach (Type addon in addons)
            {
                AddonAttribute[] attr = (AddonAttribute[])addon.GetCustomAttributes(typeof(AddonAttribute), false);

                if (attr.Length > 0)
                    if ((attr[0].Options & optionFilter) == 0)
                    {
                        pendencies.Add(addon);
                        continue;
                    }

                try
                {
                    InitializeAddon(addon);
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

            if (cache != null)
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
