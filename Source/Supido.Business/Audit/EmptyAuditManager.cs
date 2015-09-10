using Supido.Business.Context;

namespace Supido.Business.Audit
{
    /// <summary>
    /// Audit Manager that does nothing.
    /// </summary>
    public class EmptyAuditManager : BaseAuditManager
    {
        #region - Methods from BaseAuditmanager -

        /// <summary>
        /// Configures the Audit Manager
        /// </summary>
        public override void Configure()
        {
        }

        /// <summary>
        /// Commits the transaction information.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transaction information.</param>
        protected override void CommitTransac(IUserContext userContext, TransacInfo transac)
        {
        }

        /// <summary>
        /// Commits one action of the transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transac.</param>
        /// <param name="index">The index.</param>
        /// <param name="action">The action.</param>
        protected override void CommitAction(IUserContext userContext, TransacInfo transac, int index, TransacActionInfo action)
        {
        }

        /// <summary>
        /// Indicates if the action must be trailed or not.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool MustTrail(TransacActionType actionType, object source, object target)
        {
            return false;
        }


        #endregion
    }
}
