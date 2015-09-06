﻿using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Supido.Service.Cors
{
    /// <summary>
    /// Class for the Preflight Operation Behavior
    /// </summary>
    internal class PreflightOperationBehavior : IOperationBehavior
    {
        #region - Fields -

        /// <summary>
        /// The preflight operation
        /// </summary>
        private OperationDescription preflightOperation;

        /// <summary>
        /// The allowed methods
        /// </summary>
        private List<string> allowedMethods;
        
        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="PreflightOperationBehavior"/> class.
        /// </summary>
        /// <param name="preflightOperation">The preflight operation.</param>
        public PreflightOperationBehavior(OperationDescription preflightOperation)
        {
            this.preflightOperation = preflightOperation;
            this.allowedMethods = new List<string>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Add an allowed method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        public void AddAllowedMethod(string httpMethod)
        {
            this.allowedMethods.Add(httpMethod);
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The collection of objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription" />.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription" />.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new PreflightOperationInvoker(operationDescription.Messages[1].Action, this.allowedMethods);
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }
}