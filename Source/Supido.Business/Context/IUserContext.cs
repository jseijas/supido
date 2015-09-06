using Supido.Business.Audit;
using Supido.Business.BO;
using Supido.Business.DTO;
using Telerik.OpenAccess;

namespace Supido.Business.Context
{
    /// <summary>
    /// Interface for a user context.
    /// </summary>
    public interface IUserContext
    {
        #region - Properties -

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        OpenAccessContext Context { get; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        IUserDto User { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Opens a new transaction.
        /// </summary>
        void Open();

        /// <summary>
        /// Closes the current transaction.
        /// </summary>
        void Close();

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks the current transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Adds an audit trail.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="entity">The entity.</param>
        void AuditTrail(AuditOperationType type, object entity);

        /// <summary>
        /// Creates a new BO for the given DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="automanaged">if set to <c>true</c> [automanaged].</param>
        /// <returns></returns>
        ContextBO<TDto> NewBO<TDto>(bool automanaged = true);

        #endregion
    }
}
