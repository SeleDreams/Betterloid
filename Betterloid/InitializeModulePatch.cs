using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Yamaha.VOCALOID.VOCALOID5;

namespace Betterloid
{
    [HarmonyPatch(typeof(App))]
    [HarmonyPatch("InitializeModule")]
    class InitializeModulePatch
    {

        static void Postfix(ref App.ModuleResult __result, ref SplashWindow splash)
        {
            foreach (Plugin plugin in Betterloid.Instance.StartupPlugins)
            {
                try
                {
                    splash.Message = "Loading Betterloid plugin " + plugin.Config.PluginName;
                    plugin.Instance.Startup();
                }
                catch
                {
                    MessageBoxDeliverer.GeneralWarning("An error occurred while starting the plugin : " + plugin.Config.PluginName);
                }
                App.DoEvents();
            }
        }
    }
}
