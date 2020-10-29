using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace FormattedTextTestCommon.WCFExtensions
{
    public class ExceptionMarshallingErrorHandler : IErrorHandler
    {
        bool IErrorHandler.HandleError(Exception error)
        {
            if (error is FaultException)
            {
                return false; // Let WCF do normal processing
            }
            else
            {
                return true; // Fault message is already generated
            }
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
            {
                // Let WCF do normal processing
            }
            else if (error != null)
            {
                // Generate fault message manually
                // Serialize the exception into the soap message
                MessageFault messageFault = MessageFault.CreateFault(
                    new FaultCode("Sender"),
                    new FaultReason(new FaultReasonText(error.Message, CultureInfo.InvariantCulture)),
                    error,
                    new NetDataContractSerializer());
                fault = Message.CreateMessage(version, messageFault, null);
            }
        }
    }
}
