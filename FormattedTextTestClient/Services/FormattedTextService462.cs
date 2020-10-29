using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormattedTextTestCommon;

namespace FormattedTextTestClient.Services
{
    internal static class FormattedTextService462
    {
        private static IFormattedTextService formattedTextService462;

        public static IFormattedTextService Instance => formattedTextService462 ?? (formattedTextService462 =
                                                            FormattedTextServiceHelper.GetFormattedTextService(
                                                                Constants.FormattedTextService462Name));
    }
}
