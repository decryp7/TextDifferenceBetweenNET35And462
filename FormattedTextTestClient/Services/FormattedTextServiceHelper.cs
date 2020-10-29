using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FormattedTextTestCommon;
using FormattedTextTestCommon.WCFExtensions;

namespace FormattedTextTestClient.Services
{
    internal static class FormattedTextServiceHelper
    {
        public static IFormattedTextService GetFormattedTextService(string formattedTextServiceName)
        {
            NetNamedPipeBinding netNamedPipeBinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                MaxReceivedMessageSize = Constants.MaxReceivedMessageSize,
                MaxBufferSize = Constants.MaxBufferSize,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue
            };

            ChannelFactory<IFormattedTextService> channelFactory =
                new ChannelFactory<IFormattedTextService>(netNamedPipeBinding);
            channelFactory.Endpoint.Behaviors.Add(new ExceptionMarshallingBehavior());

            string address = string.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                Constants.BaseAddress, formattedTextServiceName);
            EndpointAddress endpointAddress = new EndpointAddress(address);
            return channelFactory.CreateChannel(endpointAddress);
        }
    }
}
