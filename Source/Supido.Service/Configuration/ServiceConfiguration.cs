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
        /// Gets or sets a value indicating whether this instance is HATEOAS.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is HATEOAS; otherwise, <c>false</c>.
        /// </value>
        public bool IsHateoas { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is camel case.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is camel case; otherwise, <c>false</c>.
        /// </value>
        public bool IsCamelCase { get; set; }

        /// <summary>
        /// Gets a value indicating whether [include nulls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include nulls]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeNulls { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServiceConfiguration"/> is indented.
        /// </summary>
        /// <value>
        ///   <c>true</c> if indented; otherwise, <c>false</c>.
        /// </value>
        public bool Indented { get; set; }

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
        /// Adds a new API node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public ApiNode AddNode(string path, Type dtoType, string parameterName, string parentParameterName)
        {
            return this.Root.AddNode(path, dtoType, parameterName, parentParameterName);
        }

        #endregion
    }
}
