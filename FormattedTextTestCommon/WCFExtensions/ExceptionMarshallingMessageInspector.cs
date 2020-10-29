using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace FormattedTextTestCommon.WCFExtensions
{
    public class ExceptionMarshallingMessageInspector : IClientMessageInspector
    {
        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null && reply.IsFault)
            {
                // Create a copy of the original reply to allow default processing of the message
                MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
                Message copy = buffer.CreateMessage(); // Create a copy to work with
                reply = buffer.CreateMessage(); // Restore the original message

                object faultDetail = ReadFaultDetail(copy);
                if (faultDetail is Exception exception)
                {
                    throw exception;
                }
            }
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static object ReadFaultDetail(Message reply)
        {
            const string detailElementName = "Detail";

            using (XmlDictionaryReader reader = reply.GetReaderAtBodyContents())
            {
                // Find <soap:Detail>
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == detailElementName)
                    {
                        break;
                    }
                }

                // Did we find it?
                if (reader.NodeType != XmlNodeType.Element || reader.LocalName != detailElementName)
                {
                    return null;
                }

                // Move to the contents of <soap:Detail>
                if (!reader.Read())
                {
                    return null;
                }

                try
                {
                    //Try to deserialize the exception in the soap message back to the exception object
                    //The correct assemblies where the exception is defined must be referenced by the wcf client
                    //or else this step will fail
                    return new NetDataContractSerializer().ReadObject(reader);
                }
                catch (Exception)
                {
                    // Serializer was unable to find assembly where exception is defined 
                    return null;
                }
            }
        }
    }
}
