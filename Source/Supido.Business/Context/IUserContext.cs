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
        /// Creates a new BO for the given DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="automanaged">if set to <c>true</c> [automanaged].</param>
        /// <returns></returns>
        ContextBO<TDto> NewBO<TDto>(bool automanaged = true);

        /// <summary>
        /// Trails the specified action type.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="targetObject">The target object.</param>
        void Trail(TransacActionType actionType, object sourceObject, object targetObject);

        #endregion
    }
}
