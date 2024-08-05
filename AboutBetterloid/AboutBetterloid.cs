using Betterloid;
using Betterloid.Wrappers;

namespace AboutBetterloid
{
    public class AboutBetterloid : IPlugin
    {
        public void Startup()
        {
            string info;
            info =  $"Betterloid {Betterloid.Betterloid.Instance.Config.Version} By SeleDreams.\n";
            info += $"Startup Plugins : {Betterloid.Betterloid.Instance.StartupPlugins.Count}.\n";
            info += $"Editor Plugins : {Betterloid.Betterloid.Instance.EditorPlugins.Count}.\n";
            info += $"Running on VOCALOID Version {Betterloid.Betterloid.VocaloidVersionInfo.ProductMajorPart}.{Betterloid.Betterloid.VocaloidVersionInfo.ProductMinorPart}.\n";
            MessageBoxDeliverer.GeneralInformation(info);
        }
    }
}
