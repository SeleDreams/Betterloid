using System.Reflection;
using System;
namespace BetterloidLauncher5
{
    public class Program
    {
        static Assembly betterloidassembly = Assembly.LoadFrom("Plugins/Betterloid/netframework4.6/Betterloid.dll");
        static Assembly vocaloidAssembly = Assembly.LoadFrom("VOCALOID5.exe");

        [STAThread]
        static int Main(string[] args)
        {
            Type appType = vocaloidAssembly.GetType("Yamaha.VOCALOID.VOCALOID5.App");
            object instance = Activator.CreateInstance(appType);
            var initializeComponentMethod = appType.GetMethod("InitializeComponent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            initializeComponentMethod.Invoke(instance, null);

            var betterLoidType = betterloidassembly.GetType("Betterloid.Betterloid");
            if (betterLoidType == null)
                throw new NullReferenceException();
            var betterLoidInstanceProperty = betterLoidType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            if (betterLoidInstanceProperty == null)
            {
                throw new NullReferenceException("The property was null");
            }
            var betterloidInstance = betterLoidInstanceProperty.GetValue(null);
            var setupMethod = betterLoidType.GetMethod("Setup");
            if (setupMethod == null)
                throw new NullReferenceException();
            setupMethod.Invoke(betterloidInstance, null);

            var runMethod = appType.GetMethod("Run", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            return (int)runMethod.Invoke(instance, null);
        }
    }
}
