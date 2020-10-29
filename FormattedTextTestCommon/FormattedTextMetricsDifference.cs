using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormattedTextTestCommon
{
    public class FormattedTextMetricsDifference
    {
        public double Width { get; private set; }

        public double OverhangLeading { get; private set; }

        public double OverhangTrailing { get; private set; }

        public FormattedTextMetricsDifference(FormattedTextMetrics formattedTextMetrics35,
            FormattedTextMetrics formattedTextMetrics462)
        {
            if (formattedTextMetrics35 == null)
            {
                throw new ArgumentException(nameof(formattedTextMetrics35));
            }

            if (formattedTextMetrics462 == null)
            {
                throw new ArgumentException(nameof(formattedTextMetrics462));
            }

            Width = Math.Round(formattedTextMetrics462.Width - formattedTextMetrics35.Width, 2);
            OverhangLeading =
                Math.Round(formattedTextMetrics462.OverhangLeading - formattedTextMetrics35.OverhangLeading, 2);
            OverhangTrailing =
                Math.Round(formattedTextMetrics462.OverhangTrailing - formattedTextMetrics35.OverhangTrailing, 2);
        }

    }
}
