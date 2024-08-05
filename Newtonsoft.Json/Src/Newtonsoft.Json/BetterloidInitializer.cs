using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;


public static class BetterloidInitializer
{
    static bool started = false;

    [ModuleInitializer]
    public static  void BetterloidModuleInitializer()
    {
        if (started)
        {
            return;
        }
        // Code to run when the assembly is loaded
        Assembly assembly = Assembly.LoadFrom(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Plugins/Betterloid/net6.0-windows/Betterloid.dll");
        var betterLoidType = assembly.GetType("Betterloid.Betterloid");
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
        started = true;
    }
}