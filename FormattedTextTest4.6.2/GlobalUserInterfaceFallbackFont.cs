using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using Microsoft.Win32;

namespace FormattedTextTest4._6._2
{
    public static class GlobalUserInterfaceFallbackFont
    {
        private static Uri currentGlobalUserInterfaceFallbackFontUri;

        public static string GetDefaultGlobalUserInterfaceFallbackFontLocation()
        {
            using (RegistryKey registryKey =
                Registry.LocalMachine.OpenSubKey(
                    "Software\\Microsoft\\Net Framework Setup\\NDP\\v4\\Client\\"))
            {
                string installPath = registryKey.GetValue("InstallPath") as string;
                return Path.Combine(installPath, "WPF\\Fonts\\GlobalUserInterface.CompositeFont");
            }
        }

        public static void Load(Uri compositeFontUri)
        {
            if (currentGlobalUserInterfaceFallbackFontUri == compositeFontUri)
            {
                return;
            }

            currentGlobalUserInterfaceFallbackFontUri = compositeFontUri;

            //Get the type
            Type compositeFontParserType = Type.GetType(
                "MS.Internal.FontFace.CompositeFontParser, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Type fontSourceType = Type.GetType(
                "MS.Internal.FontCache.FontSource, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Type compositeFontInfoType = Type.GetType(
                "MS.Internal.FontFace.CompositeFontInfo, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Type compositeFontFamilyType = Type.GetType(
                "MS.Internal.Shaping.CompositeFontFamily, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Type systemCompositeFontsType = Type.GetType(
                "MS.Internal.FontCache.FamilyCollection+SystemCompositeFonts, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            //Get the necessary methods
            var fontSourceConstructorInfo =
                fontSourceType.GetConstructor(new Type[] { typeof(Uri), typeof(Boolean), typeof(Boolean) });
            var fontSourceGetStreamMethodInfo = fontSourceType.GetMethod("GetStream");
            var compositeFontParserLoadXmlMethodInfo = compositeFontParserType
                .GetMethod("LoadXml", BindingFlags.NonPublic | BindingFlags.Static);
            var compositeFontFamilyTypeConstructorInfo =
                compositeFontFamilyType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new Type[] { compositeFontInfoType }, null);
            var systemCompositeFontsFieldInfo =
                systemCompositeFontsType.GetField("_systemCompositeFonts", BindingFlags.NonPublic
                                                                           | BindingFlags.Static);

            //Invoke!
            var fontSource = fontSourceConstructorInfo.Invoke(
                new object[]
                {
                    compositeFontUri,
                    true, true
                });
            var fontSourceStream = fontSourceGetStreamMethodInfo.Invoke(fontSource, null);
            var compositeFontInfo = compositeFontParserLoadXmlMethodInfo.Invoke(null, new object[] { fontSourceStream });
            var compositeFontFamily = compositeFontFamilyTypeConstructorInfo.Invoke(new object[] { compositeFontInfo });
            Array systemCompositeFonts = (Array)systemCompositeFontsFieldInfo.GetValue(null);
            systemCompositeFonts.SetValue(null, 0);
            systemCompositeFonts.SetValue(compositeFontFamily, 0);

            ClearInternalTypefaceCache();
        }

        private static void ClearInternalTypefaceCache()
        {
            #region Internal Cache location
            //From System.Windows.Media.TextFormatting.TextFormatter,
            //internal static TextFormatter FromCurrentDispatcher(TextFormattingMode textFormattingMode)
            //{
            //    Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;
            //    if (currentDispatcher == null)
            //        throw new ArgumentException(MS.Internal.PresentationCore.SR.Get("CurrentDispatcherNotFound"));
            //    TextFormatter textFormatter = textFormattingMode != TextFormattingMode.Display ? (TextFormatter)currentDispatcher.Reserved1 : (TextFormatter)currentDispatcher.Reserved4;
            //    if (textFormatter == null)
            //    {
            //        lock (TextFormatter._staticLock)
            //        {
            //            if (textFormatter == null)
            //            {
            //                textFormatter = TextFormatter.Create(textFormattingMode);
            //                if (textFormattingMode == TextFormattingMode.Display)
            //                    currentDispatcher.Reserved4 = (object)textFormatter;
            //                else
            //                    currentDispatcher.Reserved1 = (object)textFormatter;
            //            }
            //        }
            //    }
            //    Invariant.Assert(textFormatter != null);
            //    return textFormatter;
            //}
            #endregion

            //clear internal typeface cache
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (dispatcher == null)
            {
                return;
            }

            PropertyInfo reserved1Info =
                dispatcher.GetType().GetProperty("Reserved1", BindingFlags.NonPublic | BindingFlags.Instance);

            if (reserved1Info.GetValue(dispatcher) is TextFormatter)
            {
                reserved1Info.SetValue(dispatcher, null);
            }

            PropertyInfo reserved4Info =
                dispatcher.GetType().GetProperty("Reserved4", BindingFlags.NonPublic | BindingFlags.Instance);

            if (reserved4Info.GetValue(dispatcher) is TextFormatter)
            {
                reserved4Info.SetValue(dispatcher, null);
            }
        }
    }
}
