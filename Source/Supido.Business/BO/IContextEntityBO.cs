using Supido.Business.Context;
using Telerik.OpenAccess;

namespace Supido.Business.BO
{
    /// <summary>
    /// Interface for a BO that has a context manager and an OpenAccess context.
    /// </summary>
    public interface IContextEntityBO : IEntityBO
    {
        #region - Properties -

        /// <summary>
        /// Gets the context manager.
        /// </summary>
        /// <value>
        /// The context manager.
        /// </value>
        IUserContext ContextManager { get; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        OpenAccessContext Context { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic managed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic managed]; otherwise, <c>false</c>.
        /// </value>
        bool AutoManaged { get; set; }

        #endregion
    }
}
