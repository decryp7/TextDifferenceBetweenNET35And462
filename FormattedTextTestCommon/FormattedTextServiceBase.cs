using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FormattedTextTestCommon
{
    public abstract class FormattedTextServiceBase : IFormattedTextService
    {
        private readonly IVisualHost visualHost;

        protected FormattedTextServiceBase(IVisualHost visualHost)
        {
            this.visualHost = visualHost ?? throw new ArgumentNullException(nameof(visualHost));
        }

        protected abstract FormattedTextMetrics GetFormattedTextMetrics(string text, int fontSize,
            FontFamily fontFamily, FontStyle fontStyle,
            FontWeight fontWeight,
            FontStretch fontStretch);


        #region Implementation of IFormattedTextService
        public virtual FormattedTextMetrics Draw(string text, int fontSize, string fontFamily, string fontStyle, string fontWeight,
            string fontStretch, bool useNET35FallbackFont = false, bool includeAllInkInBoundingBox = false)
        {
            FontFamily fontFamilyObj = new FontFamily(fontFamily);

            FontStyle fontStyleObj = new FontStyle();
            if (!FontStyleStringToKnownStyle(fontStyle, ref fontStyleObj))
            {
                throw new ArgumentException("Unknown font style", nameof(fontStyle));
            }

            FontWeight fontWeightObj = new FontWeight();
            if (!FontWeightStringToKnownWeight(fontWeight, ref fontWeightObj))
            {
                throw new ArgumentException("Unknown font weight", nameof(fontWeight));
            }

            FontStretch fontStretchObj = new FontStretch();
            if (!FontStretchStringToKnownStretch(fontStretch, ref fontStretchObj))
            {
                throw new ArgumentException("Unknown font stretch", nameof(fontStretch));
            }

            visualHost.Draw(text, fontSize, fontFamilyObj, fontStyleObj, fontWeightObj, fontStretchObj);

            return GetFormattedTextMetrics(text, fontSize, fontFamilyObj, fontStyleObj, fontWeightObj, fontStretchObj);
        }

        #endregion

        internal static bool FontStyleStringToKnownStyle(string s, ref FontStyle fontStyle)
        {
            if (s.Equals("Normal", StringComparison.OrdinalIgnoreCase))
            {
                fontStyle = FontStyles.Normal;
                return true;
            }
            if (s.Equals("Italic", StringComparison.OrdinalIgnoreCase))
            {
                fontStyle = FontStyles.Italic;
                return true;
            }
            if (!s.Equals("Oblique", StringComparison.OrdinalIgnoreCase))
                return false;
            fontStyle = FontStyles.Oblique;
            return true;
        }

        internal static bool FontWeightStringToKnownWeight(string s, ref FontWeight fontWeight)
        {
            switch (s.Length)
            {
                case 4:
                    if (s.Equals("Bold", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Bold;
                        return true;
                    }
                    if (s.Equals("Thin", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Thin;
                        return true;
                    }
                    break;
                case 5:
                    if (s.Equals("Black", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Black;
                        return true;
                    }
                    if (s.Equals("Light", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Light;
                        return true;
                    }
                    if (s.Equals("Heavy", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Heavy;
                        return true;
                    }
                    break;
                case 6:
                    if (s.Equals("Normal", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Normal;
                        return true;
                    }
                    if (s.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Medium;
                        return true;
                    }
                    break;
                case 7:
                    if (s.Equals("Regular", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.Regular;
                        return true;
                    }
                    break;
                case 8:
                    if (s.Equals("SemiBold", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.SemiBold;
                        return true;
                    }
                    if (s.Equals("DemiBold", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.DemiBold;
                        return true;
                    }
                    break;
                case 9:
                    if (s.Equals("ExtraBold", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.ExtraBold;
                        return true;
                    }
                    if (s.Equals("UltraBold", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.UltraBold;
                        return true;
                    }
                    break;
                case 10:
                    if (s.Equals("ExtraLight", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.ExtraLight;
                        return true;
                    }
                    if (s.Equals("UltraLight", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.UltraLight;
                        return true;
                    }
                    if (s.Equals("ExtraBlack", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.ExtraBlack;
                        return true;
                    }
                    if (s.Equals("UltraBlack", StringComparison.OrdinalIgnoreCase))
                    {
                        fontWeight = FontWeights.UltraBlack;
                        return true;
                    }
                    break;
            }

            return false;
        }

        internal static bool FontStretchStringToKnownStretch(string s, ref FontStretch fontStretch)
        {
            switch (s.Length)
            {
                case 6:
                    if (s.Equals("Normal", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.Normal;
                        return true;
                    }
                    if (s.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.Medium;
                        return true;
                    }
                    break;
                case 8:
                    if (s.Equals("Expanded", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.Expanded;
                        return true;
                    }
                    break;
                case 9:
                    if (s.Equals("Condensed", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.Condensed;
                        return true;
                    }
                    break;
                case 12:
                    if (s.Equals("SemiExpanded", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.SemiExpanded;
                        return true;
                    }
                    break;
                case 13:
                    if (s.Equals("SemiCondensed", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.SemiCondensed;
                        return true;
                    }
                    if (s.Equals("ExtraExpanded", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.ExtraExpanded;
                        return true;
                    }
                    if (s.Equals("UltraExpanded", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.UltraExpanded;
                        return true;
                    }
                    break;
                case 14:
                    if (s.Equals("UltraCondensed", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.UltraCondensed;
                        return true;
                    }
                    if (s.Equals("ExtraCondensed", StringComparison.OrdinalIgnoreCase))
                    {
                        fontStretch = FontStretches.ExtraCondensed;
                        return true;
                    }
                    break;
            }

            return false;
        }

    }
}
