using System;
using System.Reflection;
using System.Windows;
namespace Betterloid.Wrappers
{
    public class MessageBoxDeliverer
    {
        private static Type _type;
        public static Type Type => _type ??= Betterloid.FindTypeRelative(".MessageBoxDeliverer");

        private static Type[] stringType = { typeof(string) };
        private static Type[] stringMessageBoxType = { typeof(string), typeof(MessageBoxResult) };

        private static MethodInfo generalokquestion;
        public static MessageBoxResult GeneralOKQuestion(string message)
        {
            generalokquestion ??= Type.GetMethod("GeneralOKQuestion", BindingFlags.Public | BindingFlags.Static, null, stringType, null);
            return (MessageBoxResult)generalokquestion.Invoke(null, new object[] { message });
        }

        private static MethodInfo generalyesnomethod;
        public static MessageBoxResult GeneralYesNoQuestion(string message, MessageBoxResult defaultResult = MessageBoxResult.Yes)
        {
            generalyesnomethod ??= Type.GetMethod("GeneralYesNoQuestion", BindingFlags.Public | BindingFlags.Static, null, stringMessageBoxType, null);
            return (MessageBoxResult)generalyesnomethod.Invoke(null, new object[] { message, defaultResult });
        }

        private static MethodInfo generalinfomethod;
        public static MessageBoxResult GeneralInformation(string message)
        {
            generalinfomethod ??= Type.GetMethod("GeneralInformation", BindingFlags.Public | BindingFlags.Static, null, stringType, null);
            return (MessageBoxResult)generalinfomethod.Invoke(null, new object[] { message });
        }

        private static MethodInfo generalwarningmethod;
        public static MessageBoxResult GeneralWarning(string message)
        {
            generalwarningmethod ??= Type.GetMethod("GeneralWarning", BindingFlags.Public | BindingFlags.Static, null, stringType, null);
            return (MessageBoxResult)generalwarningmethod.Invoke(null, new object[] {message});
        }

        private static MethodInfo generalerrormethod;
        public static MessageBoxResult GeneralError(string message)
        {
            generalerrormethod ??= Type.GetMethod("GeneralError", BindingFlags.Public | BindingFlags.Static, null, stringType, null);
            return (MessageBoxResult)generalerrormethod.Invoke(null, new object[] { message });
        }
    }
}
