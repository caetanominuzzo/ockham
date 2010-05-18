using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace primeira.Editor
{
    public class AddonManager
    {
        private static AddonDiscoveryDocument cache = null;

        private static string ADDON_DIR = "Addons";

        public static AddonHeader[] Addons
        {
            get
            {
                return cache == null ? null :
                    ( from a in cache.Addons
                      where a.BaseType != null
                      select a ).ToArray();
            }
        }

        public static DateTime CacheLastWriteTime
        {
            get
            {
                return cache == null ? DateTime.MinValue : cache.LastWriteTime;
            }
        }

        /// <summary>
        /// Discovers and initializes all available addons.
        /// 
        /// If there is a addon discovery cache file newer than the addons dir it will be loaded.
        /// </summary>
        public static void Discovery()
        {
            try
            {
                string addonDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ADDON_DIR);

                if (!Directory.Exists(addonDir))
                    Directory.CreateDirectory(addonDir);


                cache = AddonDiscoveryDocument.GetInstance();

                if (cache.Addons.Length > 0 && Directory.GetLastWriteTime(addonDir) <= cache.LastWriteTime)
                {
                    AddonHeader[] addons = cache.Addons;

                    foreach (AddonHeader assembly in addons)
                    {
                        LoadAddonHeaderFromReflection(assembly);
                    }
                }
                else
                {
                    cache.Clear();

                    string[] dlls = Directory.GetFiles(addonDir, "*.dll", SearchOption.AllDirectories);

                    foreach (string dll in dlls)
                    {
                        AddonHeader header = GetAddonHeaderReflection(dll);

                        if (header != null)
                            cache.AddHeader(header);
                    }

                    cache.ToXml();
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Log(Message_en.AddonDiscoveryError, Environment.NewLine, ex.ToString());
                throw;
            }
        }

        private static void LoadAddonHeaderFromReflection(AddonHeader header)
        {
            Assembly ass = Assembly.LoadFrom(header.AssemblyFile);

            AddonHeaderAttribute attrib = (AddonHeaderAttribute)ass.GetCustomAttributes(typeof(AddonHeaderAttribute), false).FirstOrDefault();

            if (attrib != null)
            {
                header.BaseType = ass.GetType(attrib.ClassName);

                header.Options = header.Options;

                header.GetHeaderBaseFromReflection(attrib);

                if (attrib.InitializeMethodName != null)
                    header.InitializeMethod = header.BaseType.GetMethod(attrib.InitializeMethodName);
            }
        }

        private static AddonHeader GetAddonHeaderReflection(string assemblyFile)
        {
            Assembly ass = Assembly.LoadFrom(assemblyFile);

            AddonHeaderAttribute attrib = (AddonHeaderAttribute)ass.GetCustomAttributes(typeof(AddonHeaderAttribute), false).FirstOrDefault();

            AddonHeader header = null;

            if (attrib != null)
            {
                Type t = ass.GetType(attrib.ClassName);

                if (t != null)
                {
                    header = new AddonHeader(t);

                    header.Options = attrib.Options;

                    header.GetHeaderBaseFromReflection(attrib);

                    if (attrib.InitializeMethodName != null)
                        header.InitializeMethod = header.BaseType.GetMethod(attrib.InitializeMethodName);
                }
            }

            return header;
        }

        public static void InitializeAddons()
        {
            MethodInfo[] methods = (from a in cache.Addons
                                    where a.InitializeMethod != null
                                    orderby a.Options
                                    select a.InitializeMethod).ToArray();

            foreach(MethodInfo m in methods)
            {
                InitializeAddon(m);
            }
        }

        private static void InitializeAddon(MethodInfo method)
        {
            try
            {
                method.Invoke(null, System.Type.EmptyTypes);
            }
            catch (TargetInvocationException)
            {
                throw new Exception(
                    string.Format(Message_en.AddonLoadingError, method.DeclaringType.Name));
            }
        }
    }
}
