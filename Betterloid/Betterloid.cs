using System;
using System.IO;
using Yamaha.VOCALOID.VOCALOID5;
using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;

namespace Betterloid
{
    public class Betterloid
    {
        public Harmony Harmony { get; protected set; }
        public BetterloidConfig Config { get; protected set; }
        public static Betterloid Instance { get; protected set; }

        private Betterloid()
        {
            if (Instance != null)
            {
                throw new TypeInitializationException(nameof(Betterloid), new Exception("An instance of Betterloid already exists!"));
            }
            Harmony = new Harmony("com.vocaloid.patch");
            Instance = this;
        }


        private void Initialize()
        {
            Config = JsonConvert.DeserializeObject<BetterloidConfig>(File.ReadAllText("Betterloid.json"));
        }


        private void InitializePlugins()
        {
            string[] directories = Directory.GetDirectories(Config.AddonsDir);
            foreach (string directory in directories)
            {
                string json = File.ReadAllText(directory + "/PluginConfig.json");
                PluginConfig pluginConfig = JsonConvert.DeserializeObject<PluginConfig>(json);
                if (!pluginConfig.Active)
                {
                    continue;
                }
                Assembly asm = Assembly.LoadFrom(directory + "/" + pluginConfig.PluginName);
                string typename = string.IsNullOrEmpty(pluginConfig.PluginNamespace) ? pluginConfig.PluginName : pluginConfig.PluginNamespace + "." + pluginConfig.PluginClass;
                Type t = asm.GetType(typename);
                MethodInfo m = t.GetMethod(pluginConfig.PluginMethod, BindingFlags.Public | BindingFlags.Static);
                m.Invoke(null, new object[] { this });
            }
        }


        [STAThread]
        public static void Main(string[] args)
        {
            Assembly.LoadFrom("VOCALOID5.exe");
            Betterloid betterloid = new Betterloid();
            betterloid.Initialize();
            if (betterloid.Config.AddonsActive)
            {
                betterloid.InitializePlugins();
            }
            App.Main();
        }
    }
}
