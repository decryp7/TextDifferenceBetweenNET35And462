using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormattedTextTestCommon;

namespace FormattedTextTestClient.Services
{
    internal static class FormattedTextService35
    {
        private static IFormattedTextService formattedTextService35;

        public static IFormattedTextService Instance => formattedTextService35 ?? (formattedTextService35 =
                                                            FormattedTextServiceHelper.GetFormattedTextService(
                                                                Constants.FormattedTextService35Name));
    }
}
