using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FormattedTextTestCommon;

namespace FormattedTextTestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Process formattedTextService35process;
        private readonly Process formattedTextService462process;

        public App()
        {
            EventWaitHandle formattedTextService35EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                Constants.FormattedTextService35Name);
            EventWaitHandle formattedTextService462EventWaitHandle = new EventWaitHandle(false,
                EventResetMode.AutoReset, Constants.FormattedTextService462Name);

            formattedTextService35process = Process.Start("FormattedTextTest3.5.exe");
            formattedTextService462process = Process.Start("FormattedTextTest4.6.2.exe");

            formattedTextService35EventWaitHandle.WaitOne();
            formattedTextService462EventWaitHandle.WaitOne();

            formattedTextService35EventWaitHandle.Dispose();
            formattedTextService462EventWaitHandle.Dispose();

            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            formattedTextService35process.Kill();
            formattedTextService462process.Kill();
        }
    }
}
