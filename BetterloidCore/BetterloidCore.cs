
using Betterloid;
using System.Reflection;
using System;
using System.Collections.Generic;
using Betterloid.Wrappers;
using System.Windows;
using System.Windows.Controls;
namespace BetterloidCore
{
    public class BetterloidCore : IPlugin
    {
        bool loaded = false;
        MenuItem[] GetPlugins()
        {
            List<Plugin> plugins = Betterloid.Betterloid.Instance.EditorPlugins;
            List<MenuItem> pluginItems = new List<MenuItem>();
            foreach (Plugin plugin in plugins)
            {
                MenuItem item = new MenuItem();
                item.Name = plugin.Config.PluginClass;
                item.Header = plugin.Config.PluginName;
                item.Click += (object sender, RoutedEventArgs args) => plugin.Instance.Startup();
                pluginItems.Add(item);
            }
            return pluginItems.ToArray();
        }

        public void Startup()
        {
            Window mainWindow = (Window)App.MainWindow.Object;

            if (loaded)
            {
                return;
            }
            loaded = true;
            Type mainWindowType = mainWindow.GetType();
            FieldInfo fieldInfo = mainWindowType.GetField("xMainMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            Menu menu = (fieldInfo.GetValue(mainWindow)) as Menu;
            MenuItem pluginItem = new MenuItem();
            pluginItem.Name = "PluginMenuItem";
            pluginItem.Header = "Plugins";
            MenuItem[] items = GetPlugins();
            foreach (MenuItem item in items)
            {
                pluginItem.Items.Add(item);
            }
            menu.Items.Insert(menu.Items.Count - 1, pluginItem);
        }
    }
}