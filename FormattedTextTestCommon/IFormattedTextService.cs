using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FormattedTextTestCommon
{
    [ServiceContract]
    public interface IFormattedTextService
    {
        [OperationContract]
        FormattedTextMetrics Draw(string text, int fontSize, string fontFamily, string fontStyle, string fontWeight,
            string fontStretch, bool useNET35FallbackFont = false, bool includeAllInkInBoundingBox = false);
    }
}
