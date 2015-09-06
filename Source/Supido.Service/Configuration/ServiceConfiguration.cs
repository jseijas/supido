using System;

namespace Supido.Service.Configuration
{
    /// <summary>
    /// Class for the service configuration
    /// </summary>
    public class ServiceConfiguration : IServiceConfiguration
    {
        #region - Properties -

        /// <summary>
        /// Gets the API path.
        /// </summary>
        /// <value>
        /// The API path.
        /// </value>
        public string ApiPath { get; set; }

        /// <summary>
        /// Gets a value indicating whether the service is cors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cors; otherwise, <c>false</c>.
        /// </value>
        public bool IsCors { get; set; }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public ApiNode Root { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConfiguration"/> class.
        /// </summary>
        public ServiceConfiguration()
        {
            this.Root = new ApiNode();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public ApiNode AddNode(string path, Type dtoType, string parameterName)
        {
            return this.Root.AddNode(path, dtoType, parameterName);
        }

        #endregion
    }
}
