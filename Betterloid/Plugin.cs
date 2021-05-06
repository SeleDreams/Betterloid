
namespace Betterloid
{
    public class Plugin
    {
        public Plugin(IPlugin instance, PluginConfig config)
        {
            Instance = instance;
            Config = config;
        }
        public IPlugin Instance { get; protected set; }
        public PluginConfig Config { get; protected set; }
    }
}
