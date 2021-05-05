using System;
using Yamaha.VOCALOID.VOCALOID5;
using Betterloid;

namespace AboutBetterloid
{
    public class AboutBetterloid : IPlugin
    {
        public void Startup()
        {
            string info = "Betterloid " + Betterloid.Betterloid.Instance.Config.Version + " By SeleDreams.";
            info += "\n" + "Startup Plugins : " + Betterloid.Betterloid.Instance.StartupPlugins.Count;
            info += "\n" + "Editor Plugins : " + Betterloid.Betterloid.Instance.EditorPlugins.Count;
            MessageBoxDeliverer.GeneralInformation(info);
        }
    }
}
