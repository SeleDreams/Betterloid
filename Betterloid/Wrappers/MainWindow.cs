using System;
using System.Reflection;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
namespace Betterloid.Wrappers
{
    public class MainWindow : ObjectWrapper
    {
        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".MainWindow");

        public MainWindow(Window window) : base(window)
        {

        }

        // Define a custom delegate

        private static EventInfo loadedInfo;
        public event RoutedEventHandler Loaded
        {
            add
            {
                loadedInfo ??= Type.GetEvent("Loaded");
                loadedInfo.AddEventHandler(Object, value);
            }
            remove
            {
                loadedInfo ??= Type.GetEvent("Loaded");
                loadedInfo.RemoveEventHandler(Object, value);
            }
        }


    }
}
