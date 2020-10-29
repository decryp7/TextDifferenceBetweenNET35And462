using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace FormattedTextTestCommon
{
    public static class DispatcherExtensions
    {
        public static void DoEvents(this Dispatcher dispatcher)
        {
            DispatcherFrame dispatcherFrame = new DispatcherFrame(true);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                (SendOrPostCallback) delegate(object arg)
                {
                    DispatcherFrame fr = arg as DispatcherFrame;
                    fr.Continue = false;
                }, dispatcherFrame);

            Dispatcher.PushFrame(dispatcherFrame);
        }
    }
}
