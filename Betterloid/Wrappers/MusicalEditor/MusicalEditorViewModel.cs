using System;
using System.Reflection;

namespace Betterloid.Wrappers.MusicalEditor
{
    public class MusicalEditorViewModel
    {
        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".MusicalEditor.MusicalEditorViewModel");


    }
}
