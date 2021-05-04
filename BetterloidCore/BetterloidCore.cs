using Yamaha.VOCALOID.VOCALOID5;
using HarmonyLib;
using System.Diagnostics;
using System;

namespace BetterloidCore
{
    public class BetterloidCore
    {
        public static void Execute(Betterloid.Betterloid betterloid)
        {
            betterloid.Harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(App))]
    [HarmonyPatch("InitializeModule")]
    class InitializeModulePatch
    {
        static void Postfix(ref App.ModuleResult __result, ref SplashWindow splash)
        {
            splash.Message = "This message means Betterloid is initialized, Continuing in 5 seconds !";
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(5))
            {
                App.DoEvents();
            }
            s.Stop();
        }
    }
}
