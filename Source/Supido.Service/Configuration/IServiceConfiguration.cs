
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
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        ApiNode Root { get; }

        #endregion
    }
}
