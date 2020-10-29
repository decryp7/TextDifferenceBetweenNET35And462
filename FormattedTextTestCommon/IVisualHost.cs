using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FormattedTextTestCommon
{
    public interface IVisualHost
    {
        void Draw(string text, int fontSize, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight,
            FontStretch fontStretch);
    }
}
