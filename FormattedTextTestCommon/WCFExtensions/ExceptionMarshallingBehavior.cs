using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace FormattedTextTestCommon.WCFExtensions
{
    public class ExceptionMarshallingBehavior : IServiceBehavior, IEndpointBehavior, IContractBehavior
    {
        #region IContractBehavior Members

        void IContractBehavior.AddBindingParameters(ContractDescription contract, ServiceEndpoint endpoint, BindingParameterCollection parameters)
        {
        }

        void IContractBehavior.ApplyClientBehavior(ContractDescription contract, ServiceEndpoint endpoint, ClientRuntime runtime)
        {
            this.ApplyClientBehavior(runtime);
        }

        void IContractBehavior.ApplyDispatchBehavior(ContractDescription contract, ServiceEndpoint endpoint, DispatchRuntime runtime)
        {
            if (runtime != null)
            {
                this.ApplyDispatchBehavior(runtime.ChannelDispatcher);
            }
        }

        void IContractBehavior.Validate(ContractDescription contract, ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IEndpointBehavior Members

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection parameters)
        {
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime runtime)
        {
            this.ApplyClientBehavior(runtime);
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher dispatcher)
        {
            if (dispatcher != null)
            {
                this.ApplyDispatchBehavior(dispatcher.ChannelDispatcher);
            }
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IServiceBehavior Members

        void IServiceBehavior.AddBindingParameters(ServiceDescription service, ServiceHostBase host, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription service, ServiceHostBase host)
        {
            if (host != null)
            {
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    this.ApplyDispatchBehavior(dispatcher);
                }
            }
        }

        void IServiceBehavior.Validate(ServiceDescription service, ServiceHostBase host)
        {
        }

        #endregion

        #region Private Members

        private void ApplyClientBehavior(ClientRuntime runtime)
        {
            // Don't add a message inspector if it already exists
            if (runtime.MessageInspectors.Any(inspector => inspector is ExceptionMarshallingMessageInspector))
            {
                return;
            }

            runtime.MessageInspectors.Add(new ExceptionMarshallingMessageInspector());
        }

        private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
        {
            // Don't add an error handler if it already exists
            if (dispatcher.ErrorHandlers.Any(handler => handler is ExceptionMarshallingErrorHandler))
            {
                return;
            }

            dispatcher.ErrorHandlers.Add(new ExceptionMarshallingErrorHandler());
        }

        #endregion
    }
}
