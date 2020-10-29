using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormattedTextTestCommon;

namespace FormattedTextTestClient
{
    public class ConsolidatedFormattedTextMetrics
    {
        public string @Font { get; set; }

        public double Net35_Width { get; set; }
        public double Net462_Width { get; set; }

        public double Net35_OverhangLeading { get; set; }
        public double Net462_OverhangLeading { get; set; }
        public double Difference_OverhangLeading { get; set; }

        public double Net35_OverhangTrailing { get; set; }
        public double Net462_OverhangTrailing { get; set; }
        public double Difference_OverhangTrailing { get; set; }

        public ConsolidatedFormattedTextMetrics(string font, FormattedTextMetrics net35FormattedTextMetrics,
            FormattedTextMetrics net462FormattedTextMetrics)
        {
            if (string.IsNullOrEmpty(font))
            {
                throw new ArgumentNullException(nameof(font));
            }

            if (net35FormattedTextMetrics == null)
            {
                throw new ArgumentNullException(nameof(net35FormattedTextMetrics));
            }

            if (net462FormattedTextMetrics == null)
            {
                throw new ArgumentNullException(nameof(net462FormattedTextMetrics));
            }

            Font = font;

            Net35_Width = net35FormattedTextMetrics.Width;
            Net462_Width = net462FormattedTextMetrics.Width;

            Net35_OverhangLeading = net35FormattedTextMetrics.OverhangLeading;
            Net462_OverhangLeading = net462FormattedTextMetrics.OverhangLeading;
            Difference_OverhangLeading =
                net462FormattedTextMetrics.OverhangLeading - net35FormattedTextMetrics.OverhangLeading;

            Net35_OverhangTrailing = net35FormattedTextMetrics.OverhangTrailing;
            Net462_OverhangTrailing = net462FormattedTextMetrics.OverhangTrailing;
            Difference_OverhangTrailing =
                net462FormattedTextMetrics.OverhangTrailing - net35FormattedTextMetrics.OverhangTrailing;
        }
    }
}
