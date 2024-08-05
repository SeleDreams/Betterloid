using System;
using System.Reflection;
using System.Windows;

namespace Betterloid.Wrappers
{
    public class App
    {
        public object Object { get; private set; }

        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".App");

        private static App _instance;
        public static App Shared => _instance ??= new App(Type.GetProperty("Shared").GetValue(null));
        private static AudioPlayer _audioPlayer;
        public static AudioPlayer AudioPlayer => _audioPlayer ??= new AudioPlayer(Type.GetProperty("AudioPlayer").GetValue(null));

        private static MainWindow _window_instance;
        public static MainWindow MainWindow => _window_instance ??= new MainWindow((Window)Type.GetProperty("MainWindow").GetValue(Shared.Object));

        public App(object o)
        {
            Object = o;
        }

        private static EventInfo activatedInfo;
        public event EventHandler Activated { 
            add {
                activatedInfo ??= Type.GetEvent("Activated");
                activatedInfo.AddEventHandler(Object, value);
            }
            remove
            {
                activatedInfo ??= Type.GetEvent("Activated");
                activatedInfo.RemoveEventHandler(Object, value);
            }
        }

        public static void DoEvents()
        {
            var method = Type.GetMethod("DoEvents");
            method.Invoke(null, null);
        }
    }
}
