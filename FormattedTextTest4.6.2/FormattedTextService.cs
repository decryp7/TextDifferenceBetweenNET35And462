using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FormattedTextTestCommon;
using FormattedTextTestCommon.Views;

namespace FormattedTextTest4._6._2
{
    public class FormattedTextService : FormattedTextServiceBase
    {
        public FormattedTextService(VisualHost visualHost) : base(visualHost)
        {
        }

        #region Overrides of FormattedTextServiceBase

        protected override FormattedTextMetrics GetFormattedTextMetrics(string text, int fontSize,
            FontFamily fontFamily, FontStyle fontStyle,
            FontWeight fontWeight, FontStretch fontStretch)
        {
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            System.Windows.DpiScale dpiScale =
                System.Windows.Media.VisualTreeHelper.GetDpi(Application.Current.MainWindow);
            double pixelsPerDip = dpiScale.PixelsPerDip;

            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black, pixelsPerDip);

            return new FormattedTextMetrics(formattedText.Width,
                formattedText.OverhangLeading,
                formattedText.OverhangTrailing);
        }

        public override FormattedTextMetrics Draw(string text, int fontSize, string fontFamily, string fontStyle,
            string fontWeight,
            string fontStretch, bool useNET35FallbackFont = false, bool includeAllInkInBoundingBox = false)
        {
            NET35CompatibilityPreferences.IncludeAllInkInBoundingBox = includeAllInkInBoundingBox;
            bool value = NET35CompatibilityPreferences.IncludeAllInkInBoundingBox;

            if (useNET35FallbackFont)
            {
                GlobalUserInterfaceFallbackFont.Load(new Uri(
                    @"pack://application:,,,/FormattedTextTestCommon;component/Resources/NET35GlobalUserInterface.CompositeFont"));
            }
            else
            {
                GlobalUserInterfaceFallbackFont.Load(new Uri(GlobalUserInterfaceFallbackFont
                    .GetDefaultGlobalUserInterfaceFallbackFontLocation()));
            }

            return base.Draw(text, fontSize, fontFamily, fontStyle, fontWeight, fontStretch, useNET35FallbackFont,
                includeAllInkInBoundingBox);
        }

        #endregion
    }
}
