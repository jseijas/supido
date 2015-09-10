using Supido.Business.Context;
using System;

namespace Supido.Business.Audit
{
    /// <summary>
    /// Base class for an audit manager
    /// </summary>
    public abstract class BaseAuditManager : IAuditManager
    {
        #region - Methods -

        #region - Abstract Methods -

        /// <summary>
        /// Configures the Audit Manager
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Indicates if the action must be trailed or not.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public abstract bool MustTrail(TransacActionType actionType, object source, object target);

        /// <summary>
        /// Commits the transaction information.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transaction information.</param>
        protected abstract void CommitTransac(IUserContext userContext, TransacInfo transac);

        /// <summary>
        /// Commits one action of the transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transac.</param>
        /// <param name="index">The index.</param>
        /// <param name="action">The action.</param>
        protected abstract void CommitAction(IUserContext userContext, TransacInfo transac, int index, TransacActionInfo action);

        #endregion

        #region - Protected Methods -

        /// <summary>
        /// Commits the transaction information and all the actions.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transac.</param>
        protected virtual void CommitFullTransac(IUserContext userContext, TransacInfo transac) 
        {
            this.CommitTransac(userContext, transac);
            for (int i = 0; i < transac.Actions.Count; i++)
            {
                this.CommitAction(userContext, transac, i, transac.Actions[i]);
            }
        }

        #endregion

        #region - Public Methods -

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns></returns>
        public TransacInfo StartTransaction(IUserContext userContext)
        {
            TransacInfo result = new TransacInfo();
            result.TransacId = null;
            result.UserId = userContext.User.UserId.Value;
            result.StartDateTime = DateTime.UtcNow;
            result.EndDateTime = null;
            return result;
        }

        /// <summary>
        /// Ends the transaction.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transac.</param>
        public void EndTransaction(IUserContext userContext, TransacInfo transac)
        {
            transac.EndDateTime = DateTime.UtcNow;
            this.CommitFullTransac(userContext, transac);
        }

        #endregion

        #endregion
    }
}
