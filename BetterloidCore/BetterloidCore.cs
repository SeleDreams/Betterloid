using System.Windows;
using System.Windows.Controls;
using Betterloid;
using Yamaha.VOCALOID.VOCALOID5;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace BetterloidCore
{
    public class BetterloidCore : IPlugin
    {
        MenuItem[] GetPlugins()
        {
            List<Plugin> plugins = Betterloid.Betterloid.Instance.EditorPlugins;
            MenuItem[] pluginItems = new MenuItem[plugins.Count];
            for (int i = 0; i < plugins.Count; i++)
            {
                pluginItems[i] = new MenuItem();
                pluginItems[i].Name = plugins[i].Config.PluginClass;
                pluginItems[i].Header = plugins[i].Config.PluginName;
                pluginItems[i].Click += (object sender,RoutedEventArgs args) => plugins[i - 1].Instance.Startup();
            }
            return pluginItems;
        }

        public void Startup()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Loaded += (object e, RoutedEventArgs args) => {
                Type mainWindowType = mainWindow.GetType();
                FieldInfo fieldInfo = mainWindowType.GetField("xMainMenu",BindingFlags.Instance | BindingFlags.NonPublic);
                Menu menu = (fieldInfo.GetValue(mainWindow)) as Menu;
                MenuItem pluginItem = new MenuItem();
                pluginItem.Name = "PluginMenuItem";
                pluginItem.Header = "Plugins";
                MenuItem[] items = GetPlugins();
                foreach (MenuItem item in items)
                {
                    pluginItem.Items.Add(item);
                }
                menu.Items.Insert(menu.Items.Count - 1,pluginItem);
            };
        }
    }
}
