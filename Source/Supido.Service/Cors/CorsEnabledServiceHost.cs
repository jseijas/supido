using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Supido.Service.Cors
{
    /// <summary>
    /// Service Host for CORS enabled services.
    /// </summary>
    public class CorsEnabledServiceHost : ServiceHost
    {
        #region - Fields -

        /// <summary>
        /// The contract type
        /// </summary>
        private Type contractType;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsEnabledServiceHost"/> class.
        /// </summary>
        /// <param name="serviceType">The type of hosted service.</param>
        /// <param name="baseAddresses">An array of type <see cref="T:System.Uri" /> that contains the base addresses for the hosted service.</param>
        public CorsEnabledServiceHost(Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            this.contractType = GetContractType(serviceType);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Invoked during the transition of a communication object into the opening state.
        /// </summary>
        protected override void OnOpening()
        {
            ServiceEndpoint endpoint = this.AddServiceEndpoint(this.contractType, new WebHttpBinding(), "");

            List<OperationDescription> corsEnabledOperations = endpoint.Contract.Operations
                .Where(o => o.Behaviors.Find<CorsEnabledAttribute>() != null)
                .ToList();

            AddPreflightOperations(endpoint, corsEnabledOperations);

            endpoint.Behaviors.Add(new WebHttpBehavior());
            endpoint.Behaviors.Add(new EnableCorsEndpointBehavior());

            base.OnOpening();
        }

        /// <summary>
        /// Gets the type of the contract.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// Service type  + serviceType.FullName +  does not implement any interface decorated with the ServiceContractAttribute.
        /// or
        /// Service type  + serviceType.FullName +  implements multiple interfaces decorated with the ServiceContractAttribute, not supported by this factory.
        /// </exception>
        private Type GetContractType(Type serviceType)
        {
            if (HasServiceContract(serviceType))
            {
                return serviceType;
            }

            Type[] possibleContractTypes = serviceType.GetInterfaces().Where(i => !i.Name.StartsWith("IContextService") && !i.Name.StartsWith("ISimpleService") && HasServiceContract(i)).ToArray();

            switch (possibleContractTypes.Length)
            {
                case 0:
                    throw new InvalidOperationException("Service type " + serviceType.FullName + " does not implement any interface decorated with the ServiceContractAttribute.");
                case 1:
                    return possibleContractTypes[0];
                default:
                    throw new InvalidOperationException("Service type " + serviceType.FullName + " implements multiple interfaces decorated with the ServiceContractAttribute, not supported by this factory.");
            }
        }

        /// <summary>
        /// Determines whether [has service contract] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static bool HasServiceContract(Type type)
        {
            return Attribute.IsDefined(type, typeof(ServiceContractAttribute), false);
        }

        /// <summary>
        /// Adds the preflight operations.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="corsOperations">The cors operations.</param>
        private void AddPreflightOperations(ServiceEndpoint endpoint, List<OperationDescription> corsOperations)
        {
            Dictionary<string, PreflightOperationBehavior> uriTemplates = new Dictionary<string, PreflightOperationBehavior>(StringComparer.OrdinalIgnoreCase);

            foreach (var operation in corsOperations)
            {
                if ((operation.Behaviors.Find<WebGetAttribute>() != null) || (operation.IsOneWay))
                {
                    continue;
                }

                string originalUriTemplate;
                WebInvokeAttribute originalWia = operation.Behaviors.Find<WebInvokeAttribute>();

                if (originalWia != null && originalWia.UriTemplate != null)
                {
                    originalUriTemplate = NormalizeTemplate(originalWia.UriTemplate);
                }
                else
                {
                    originalUriTemplate = operation.Name;
                }

                string originalMethod = originalWia != null && originalWia.Method != null ? originalWia.Method : "POST";

                if (uriTemplates.ContainsKey(originalUriTemplate))
                {
                    PreflightOperationBehavior operationBehavior = uriTemplates[originalUriTemplate];
                    operationBehavior.AddAllowedMethod(originalMethod);
                }
                else
                {
                    ContractDescription contract = operation.DeclaringContract;
                    OperationDescription preflightOperation = new OperationDescription(operation.Name + CorsConstants.PreflightSuffix, contract);
                    MessageDescription inputMessage = new MessageDescription(operation.Messages[0].Action + CorsConstants.PreflightSuffix, MessageDirection.Input);
                    inputMessage.Body.Parts.Add(new MessagePartDescription("input", contract.Namespace) { Index = 0, Type = typeof(Message) });
                    preflightOperation.Messages.Add(inputMessage);
                    MessageDescription outputMessage = new MessageDescription(operation.Messages[1].Action + CorsConstants.PreflightSuffix, MessageDirection.Output);
                    outputMessage.Body.ReturnValue = new MessagePartDescription(preflightOperation.Name + "Return", contract.Namespace) { Type = typeof(Message) };
                    preflightOperation.Messages.Add(outputMessage);

                    WebInvokeAttribute wia = new WebInvokeAttribute();
                    wia.UriTemplate = originalUriTemplate;
                    wia.Method = "OPTIONS";

                    preflightOperation.Behaviors.Add(wia);
                    preflightOperation.Behaviors.Add(new DataContractSerializerOperationBehavior(preflightOperation));
                    PreflightOperationBehavior preflightOperationBehavior = new PreflightOperationBehavior(preflightOperation);
                    preflightOperationBehavior.AddAllowedMethod(originalMethod);
                    preflightOperationBehavior.AddAllowedMethod("DELETE");
                    preflightOperation.Behaviors.Add(preflightOperationBehavior);
                    uriTemplates.Add(originalUriTemplate, preflightOperationBehavior);

                    contract.Operations.Add(preflightOperation);
                }
            }
        }

        /// <summary>
        /// Normalizes the template.
        /// </summary>
        /// <param name="uriTemplate">The URI template.</param>
        /// <returns></returns>
        private string NormalizeTemplate(string uriTemplate)
        {
            int queryIndex = uriTemplate.IndexOf('?');
            if (queryIndex >= 0)
            {
                uriTemplate = uriTemplate.Substring(0, queryIndex);
            }

            int paramIndex;
            while ((paramIndex = uriTemplate.IndexOf('{')) >= 0)
            {
                int endParamIndex = uriTemplate.IndexOf('}', paramIndex);
                if (endParamIndex >= 0)
                {
                    uriTemplate = uriTemplate.Substring(0, paramIndex) + '*' + uriTemplate.Substring(endParamIndex + 1);
                }
            }

            return uriTemplate;
        }

        #endregion
    }
}