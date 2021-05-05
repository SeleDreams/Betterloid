using System;
using System.IO;
using Yamaha.VOCALOID.VOCALOID5;
using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Betterloid
{
    public class Betterloid
    {
        public Harmony Harmony { get; protected set; }
        public BetterloidConfig Config { get; protected set; }
        public List<Plugin> StartupPlugins { get; protected set; }
        public List<Plugin> EditorPlugins { get; protected set; }

        public static Betterloid Instance { get; protected set; }


        private Betterloid()
        {
            if (Instance != null)
            {
                throw new TypeInitializationException(nameof(Betterloid), new Exception("An instance of Betterloid already exists!"));
            }
            Harmony = new Harmony("com.vocaloid.patch");
            Instance = this;
            StartupPlugins = new List<Plugin>();
            EditorPlugins = new List<Plugin>();
        }


        private void Initialize()
        {
            Config = JsonConvert.DeserializeObject<BetterloidConfig>(File.ReadAllText("Betterloid.json"));
        }


        public void InitializePlugins()
        {
            string[] directories = Directory.GetDirectories(Config.PluginsDir);
            foreach (string directory in directories)
            {
                string json = File.ReadAllText(directory + "/PluginConfig.json");
                PluginConfig pluginConfig = JsonConvert.DeserializeObject<PluginConfig>(json);

                Assembly asm = Assembly.LoadFrom(directory + "/" + pluginConfig.PluginAssembly);
                string typename = string.IsNullOrEmpty(pluginConfig.PluginNamespace) ? pluginConfig.PluginClass : pluginConfig.PluginNamespace + "." + pluginConfig.PluginClass;
                Type t = asm.GetType(typename);

                if (!pluginConfig.PluginActive)
                {
                    continue;
                }

                IPlugin plugin = (IPlugin)Activator.CreateInstance(t);

                if ( pluginConfig.PluginType == "Startup")
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
            betterloid.Harmony.PatchAll();
            App.Main();
        }
    }
}
