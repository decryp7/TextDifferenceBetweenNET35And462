using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FormattedTextTestCommon;
using FormattedTextTestCommon.WCFExtensions;

namespace FormattedTextTest4._6._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceHost serviceHost;
        private readonly string runtimeVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
        private readonly string runtimeDirectory =
            System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += OnClosed;
            serviceHost = StartService();

            EventWaitHandle wh = EventWaitHandle.OpenExisting(Constants.FormattedTextService462Name);
            wh.Set();
            wh.Close();

            this.Title = FormattableString.Invariant($"{runtimeVersion} from {runtimeDirectory}.");
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
                Constants.BaseAddress, Constants.FormattedTextService462Name);
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
