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

        #endregion
    }
}
