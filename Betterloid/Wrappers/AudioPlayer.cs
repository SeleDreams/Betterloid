using System;
using System.Reflection;
namespace Betterloid.Wrappers
{
    public enum VEConnectMode : short
    {
        ReWireSlave = 1,
        StandAlone = 2,
        VST = 4,
        VSTWithReWire = 8,
        VSTWithARA = 16,
        VSTRequest = 12,
        App = 3
    }
    public class AudioPlayer : ObjectWrapper
    {

        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".AudioPlayer");

        public AudioPlayer(object o) : base(o)
        {
        }
        
        
        // Properties
        

        static private PropertyInfo _connectMode;

        /// <summary>
        /// Defines the connection mode
        /// </summary>
        public VEConnectMode ConnectMode
        {
            get
            {
                _connectMode ??= Type.GetProperty("ConnectMode");
                return (VEConnectMode)_connectMode.GetValue(Object);
            }
            set
            {
                _connectMode ??= Type.GetProperty("ConnectMode");
                _connectMode.SetValue(Object,value);
            }
        }
    }
}
