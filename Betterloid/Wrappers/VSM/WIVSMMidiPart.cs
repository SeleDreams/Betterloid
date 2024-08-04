using System;
using System.Reflection;

namespace Betterloid.Wrappers.VSM
{
    public class WIVSMMidiPart
    {
        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".VSM.WIVSMMidiPart");

        public object Object { get; private set; }
        public WIVSMMidiPart(object o )
        {
            Object = o;
        }

        private static PropertyInfo _parentProperty;
        public WIVSMMidiPart Next => new WIVSMMidiPart((_parentProperty ??= Type.GetProperty("Next")).GetValue(Object));

        private static PropertyInfo _prevProperty;
        public WIVSMMidiPart Prev => new WIVSMMidiPart((_prevProperty ??= Type.GetProperty("Prev")).GetValue(Object));

    }
}
