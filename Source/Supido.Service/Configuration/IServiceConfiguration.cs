using System;

namespace Supido.Service.Configuration
{
    /// <summary>
    /// Interface for the service configuration.
    /// </summary>
    public interface IServiceConfiguration
    {
        #region - Properties -

        /// <summary>
        /// Gets the API path.
        /// </summary>
        /// <value>
        /// The API path.
        /// </value>
        string ApiPath { get; }

        /// <summary>
        /// Gets a value indicating whether the service is cors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cors; otherwise, <c>false</c>.
        /// </value>
        bool IsCors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is HATEOAS.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is HATEOAS; otherwise, <c>false</c>.
        /// </value>
        bool IsHateoas { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is camel case.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is camel case; otherwise, <c>false</c>.
        /// </value>
        bool IsCamelCase { get; }

        /// <summary>
        /// Gets a value indicating whether [include nulls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include nulls]; otherwise, <c>false</c>.
        /// </value>
        bool IncludeNulls { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServiceConfiguration"/> is indented.
        /// </summary>
        /// <value>
        ///   <c>true</c> if indented; otherwise, <c>false</c>.
        /// </value>
        bool Indented { get; }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        ApiNode Root { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a new API node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parentParameterName">Name of the parent parameter.</param>
        /// <returns></returns>
        ApiNode AddNode(string path, Type dtoType, string parameterName, string parentParameterName);

        #endregion
    }
}
