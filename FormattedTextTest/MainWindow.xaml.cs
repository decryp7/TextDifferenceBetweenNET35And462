using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows;
using FormattedTextTestCommon;
using FormattedTextTestCommon.WCFExtensions;

namespace FormattedTextTest3._5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceHost serviceHost;

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += OnClosed;
            serviceHost = StartService();

            EventWaitHandle wh = EventWaitHandle.OpenExisting(Constants.FormattedTextService35Name);
            wh.Set();
            wh.Close();
        }

        private ServiceHost StartService()
        {
            ServiceHost serviceHost = new ServiceHost(new FormattedTextService(VisualHost));

            NetNamedPipeBinding netNamedPipeBinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                MaxReceivedMessageSize = Constants.MaxReceivedMessageSize,
                MaxBufferSize = Constants.MaxBufferSize,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue
            };

            string address = string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                Constants.BaseAddress, Constants.FormattedTextService35Name);
            ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(IFormattedTextService),
                netNamedPipeBinding, address);
            serviceEndpoint.Behaviors.Add(new ExceptionMarshallingBehavior());
            serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>().InstanceContextMode =
                InstanceContextMode.Single;
            serviceHost.Open();

            return serviceHost;
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            serviceHost.Close();
        }
    }
}
