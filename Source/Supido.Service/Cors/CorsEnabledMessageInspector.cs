using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Supido.Service.Cors
{
    /// <summary>
    /// Message inspector for the dispatching of CORS enabled messages
    /// </summary>
    internal class CorsEnabledMessageInspector : IDispatchMessageInspector
    {
        #region - Fields -

        /// <summary>
        /// The cors enabled operation names
        /// </summary>
        private List<string> corsEnabledOperationNames;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsEnabledMessageInspector"/> class.
        /// </summary>
        /// <param name="corsEnabledOperations">The cors enabled operations.</param>
        public CorsEnabledMessageInspector(List<OperationDescription> corsEnabledOperations)
        {
            this.corsEnabledOperationNames = corsEnabledOperations.Select(o => o.Name).ToList();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// </returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            HttpRequestMessageProperty httpProp = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            object operationName;
            request.Properties.TryGetValue(WebHttpDispatchOperationSelector.HttpOperationNamePropertyName, out operationName);
            if (httpProp != null && operationName != null && this.corsEnabledOperationNames.Contains((string)operationName))
            {
                string origin = httpProp.Headers[CorsConstants.Origin];
                if (origin != null)
                {
                    return origin;
                }
            }

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" /> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            string origin = correlationState as string;
            if (origin != null)
            {
                HttpResponseMessageProperty httpProp = null;
                if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    httpProp = (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];
                }
                else
                {
                    httpProp = new HttpResponseMessageProperty();
                    reply.Properties.Add(HttpResponseMessageProperty.Name, httpProp);
                }

                httpProp.Headers.Add(CorsConstants.AccessControlAllowOrigin, origin);
            }
        }

        #endregion
    }
}