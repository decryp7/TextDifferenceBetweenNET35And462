using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FormattedTextTestCommon;
using FormattedTextTestCommon.Views;

namespace FormattedTextTest3._5
{
    internal class FormattedTextService : FormattedTextServiceBase
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

            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);

            return new FormattedTextMetrics(formattedText.Width, formattedText.OverhangLeading,
                formattedText.OverhangTrailing);
        }

        #endregion
    }
}
