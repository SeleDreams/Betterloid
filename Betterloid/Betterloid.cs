using System;
using System.IO;
using Yamaha.VOCALOID.VOCALOID5;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using System.Threading;
using System.Linq;

namespace Betterloid
{
    public class Betterloid
    {
        public BetterloidConfig Config { get; protected set; }
        public Assembly NewtonsoftJSON { get; protected set; }
        public List<Plugin> StartupPlugins { get; protected set; }
        public List<Plugin> EditorPlugins { get; protected set; }
        public static Betterloid Instance { get; protected set; }

        private Betterloid()
        {
            if (Instance != null)
            {
                throw new TypeInitializationException(nameof(Betterloid), new Exception("An instance of Betterloid already exists!"));
            }
            Instance = this;
            StartupPlugins = new List<Plugin>();
            EditorPlugins = new List<Plugin>();
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

        private void Initialize()
        {
            NewtonsoftJSON = Assembly.LoadFrom("Newtonsoft.Json.dll");
            Config = DeserializeObject<BetterloidConfig>(File.ReadAllText("Betterloid.json"));
        }

        public void InitializePlugins()
        {
            string[] directories = Directory.GetDirectories(Config.PluginsDir);
            foreach (string directory in directories)
            {
                string json = File.ReadAllText(directory + "/PluginConfig.json");
                PluginConfig pluginConfig = DeserializeObject<PluginConfig>(json);

                Assembly asm = Assembly.LoadFrom(directory + "/" + pluginConfig.PluginAssembly);
                string typename = string.IsNullOrEmpty(pluginConfig.PluginNamespace) ? pluginConfig.PluginClass : pluginConfig.PluginNamespace + "." + pluginConfig.PluginClass;
                Type t = asm.GetType(typename);

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
        [STAThread]
        public static void Main(string[] args)
        {
            Assembly.LoadFrom("VOCALOID5.exe");
            Betterloid betterloid = new Betterloid();
            betterloid.Initialize();
            betterloid.InitializePlugins();
            App app = new App();
            app.InitializeComponent();

            app.Activated += (object sender, EventArgs arg) =>
            {
                foreach (Plugin plugin in Instance.StartupPlugins)
                {
                    try
                    {
                        plugin.Instance.Startup();
                    }
                    catch
                    {
                        MessageBoxDeliverer.GeneralWarning("An error occurred while starting the plugin : " + plugin.Config.PluginName);
                    }
                    App.DoEvents();
                }

            };
            app.Run();
        }
    }
}
