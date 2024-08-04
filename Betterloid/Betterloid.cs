using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Betterloid.Wrappers;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
namespace Betterloid
{
    public enum VocaloidVersions
    {
        VOCALOID5,
        VOCALOID6
    }

    public class Betterloid
    {
        public BetterloidConfig Config { get; protected set; }
        public Assembly NewtonsoftJSON { get; protected set; }
        public List<Plugin> StartupPlugins { get; protected set; }
        public List<Plugin> EditorPlugins { get; protected set; }

        public static Assembly VocaloidAssembly => AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("VOCALOID"));

        public static string BasePath => Path.GetDirectoryName(VocaloidAssembly.Location);
        public static FileVersionInfo VocaloidVersionInfo => FileVersionInfo.GetVersionInfo(VocaloidAssembly.Location);

        private static Betterloid _instance;
        public static Betterloid Instance => _instance ??= new Betterloid();

        public Betterloid()
        {
            StartupPlugins = new List<Plugin>();
            EditorPlugins = new List<Plugin>();
        }

        public static Type FindTypeRelative(string typeName)
        {
            return VocaloidAssembly.ExportedTypes.FirstOrDefault(t => t.FullName.StartsWith("Yamaha.VOCALOID") && t.FullName.EndsWith(typeName));
        }

        public static T DeserializeObject<T>(string json)
        {
            Type jsonconvert = Instance.NewtonsoftJSON.GetType("Newtonsoft.Json.JsonConvert");
            var deserializer = jsonconvert.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(i => i.Name.Equals("DeserializeObject", StringComparison.InvariantCulture))
        .Where(i => i.IsGenericMethod)
        .Where(i => i.GetParameters().Select(a => a.ParameterType).SequenceEqual(new Type[] { typeof(string) }))
        .Single();
            var genericMethod = deserializer.MakeGenericMethod(new Type[] { typeof(T) });
            return (T)genericMethod.Invoke(null, new object[] { json });
        }

        public static string SerializeObject<T>(T source)
        {
            Type jsonconvert = Instance.NewtonsoftJSON.GetType("Newtonsoft.Json.JsonConvert");
            var serializer = jsonconvert.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(i => i.Name.Equals("SerializeObject", StringComparison.InvariantCulture))
        .Where(i => i.GetParameters().Select(a => a.ParameterType).SequenceEqual(new Type[] { typeof(object) }))
        .Single();
            return (string)serializer.Invoke(null, new object[] { source });
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("VOCALOID6"))
            {
                return VocaloidAssembly;
            }
            return null;
        }
        public void Initialize()
        {
            NewtonsoftJSON = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("Newtonsoft.Json"));
            if (NewtonsoftJSON == null)
            {
                NewtonsoftJSON = Assembly.LoadFrom(BasePath + "/Newtonsoft.Json.dll");
            }
            Config = DeserializeObject<BetterloidConfig>(File.ReadAllText(BasePath + "/Betterloid.json"));
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        public void Setup()
        {
            Initialize();
            try
            {
                InitializePlugins();
            }
            catch (Exception ex)
            {
                MessageBoxDeliverer.GeneralError(ex.Message);
            }

           
            bool started = false;
            EventHandler lambda = (object sender, EventArgs args) =>
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    if (started)
                    {
                        return;
                    }
                    started = true;
                    foreach (Plugin plugin in Instance.StartupPlugins)
                    {
                        try
                        {
                            plugin.Instance.Startup();
                        }
                        catch (Exception ex)
                        {
                            MessageBoxDeliverer.GeneralWarning("An error occurred while starting the plugin : " + plugin.Config.PluginName);
                            MessageBoxDeliverer.GeneralError(ex.ToString());
                        }
                    }
                }));
            };
            /*if (VocaloidVersionInfo.ProductMajorPart == 6)
            {
                Window app = App.MainWindow.Object as Window;
                if (app == null)
                {
                    MessageBoxDeliverer.GeneralError("The app is null!");
                    return;
                }
                app.Activated += lambda;
            }
            else if (VocaloidVersionInfo.ProductMajorPart == 5)
            {
                App app = App.Shared;
                app.Activated += lambda;
            }*/
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if (started)
                {
                    return;
                }
                started = true;
                foreach (Plugin plugin in Instance.StartupPlugins)
                {
                    try
                    {
                        plugin.Instance.Startup();
                    }
                    catch (Exception ex)
                    {
                        MessageBoxDeliverer.GeneralWarning("An error occurred while starting the plugin : " + plugin.Config.PluginName);
                        MessageBoxDeliverer.GeneralError(ex.ToString());
                    }
                }
            }));

        }

        public void InitializePlugins()
        {
            string[] directories = Directory.GetDirectories(BasePath + "/" + Config.PluginsDir);
            foreach (string directory in directories)
            {
                if (!File.Exists(directory + "/PluginConfig.json"))
                {
                    if (Path.GetFileName(directory) != "Betterloid")
                    {
                        MessageBoxDeliverer.GeneralError(directory + "/PluginConfig.json" + " does not exist!");
                    }
                    continue;
                }

                string json = File.ReadAllText(directory + "/PluginConfig.json");
                PluginConfig pluginConfig = DeserializeObject<PluginConfig>(json);
                Assembly asm;
                if (VocaloidVersionInfo.ProductMajorPart == 5)
                {
                    if (string.IsNullOrEmpty(pluginConfig.PluginAssemblyV5))
                    {
                        MessageBoxDeliverer.GeneralError($"The plugin {pluginConfig.PluginName} does not appear to support VOCALOID5. This plugin will not run.");
                        continue;
                    }
                    asm = Assembly.LoadFrom(directory + "/" + pluginConfig.PluginAssemblyV5);
                }
                else if (VocaloidVersionInfo.ProductMajorPart == 6)
                {
                    if (string.IsNullOrEmpty(pluginConfig.PluginAssemblyV6))
                    {
                        MessageBoxDeliverer.GeneralError($"The plugin {pluginConfig.PluginName} does not appear to support VOCALOID6. This plugin will not run.");
                        continue;
                    }
                    asm = Assembly.LoadFrom(directory + "/" + pluginConfig.PluginAssemblyV6);
                }
                else
                {
                    continue;
                }
                string typename = string.IsNullOrEmpty(pluginConfig.PluginNamespace) ? pluginConfig.PluginClass : pluginConfig.PluginNamespace + "." + pluginConfig.PluginClass;
                Type t = asm.GetType(typename);
                if (t == null)
                {
                    MessageBoxDeliverer.GeneralError($"The plugin {pluginConfig.PluginName} failed to load.");
                    continue;
                }

                if (!pluginConfig.PluginActive)
                {
                    continue;
                }

                IPlugin plugin = (IPlugin)Activator.CreateInstance(t);

                if (pluginConfig.PluginType == "Startup")
                {
                    StartupPlugins.Add(new Plugin(plugin, pluginConfig));
                }
                else
                {
                    EditorPlugins.Add(new Plugin(plugin, pluginConfig));
                }
            }
        }
    }
}
