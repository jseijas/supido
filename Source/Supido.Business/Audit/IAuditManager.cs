using Supido.Business.Context;

namespace Supido.Business.Audit
{
    /// <summary>
    /// Interface for an Audit Manager
    /// </summary>
    public interface IAuditManager
    {
        #region - Methods -

        /// <summary>
        /// Configures the Audit Manager
        /// </summary>
        void Configure();

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns></returns>
        TransacInfo StartTransaction(IUserContext userContext);

        /// <summary>
        /// Ends the transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transac.</param>
        void EndTransaction(IUserContext userContext, TransacInfo transac);

        /// <summary>
        /// Indicates if the action must be trailed or not.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        bool MustTrail(TransacActionType actionType, object source, object target);

        #endregion
    }
}
