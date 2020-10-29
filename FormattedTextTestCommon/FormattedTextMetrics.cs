using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FormattedTextTestCommon
{
    [DataContract]
    public class FormattedTextMetrics
    {
        [DataMember]
        public double OverhangTrailing { get; private set; }

        [DataMember]
        public double OverhangLeading { get; private set; }

        [DataMember]
        public double Width { get; private set; }

        public FormattedTextMetrics(double width, double overhangLeading, double overhangTrailing)
        {
            Width = Math.Round(width, 2);
            OverhangLeading = Math.Round(overhangLeading, 2);
            OverhangTrailing = Math.Round(overhangTrailing, 2);
        }
    }
}
