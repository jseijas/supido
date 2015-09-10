using Supido.Business;
using Supido.Business.Audit;
using Supido.Business.Context;
using Supido.Business.Meta;
using Supido.Core.Container;
using Supido.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Supido.Core.Utils;

namespace Supido.Demo.Service.Security
{
    /// <summary>
    /// Class for the Audit Manager
    /// </summary>
    public class AuditManager : BaseAuditManager
    {
        #region - Methods -

        #region - Private Methods -

        /// <summary>
        /// Converts from string to Audit Type
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        private AuditType StrToAuditType(string str)
        {
            return str.ToLower().ToEnum<AuditType>();
        }

        #endregion

        #region - Methods from BaseAuditManager -

        /// <summary>
        /// Configures the Audit Manager
        /// </summary>
        public override void Configure()
        {
            using (EntitiesModel context = new EntitiesModel())
            {
                IMetamodelManager metamodelManager = IoC.Get<ISecurityManager>().MetamodelManager;
                IList<MetaEntity> metaEntities = context.MetaEntities.ToList();
                foreach (MetaEntity metaEntity in metaEntities)
                {
                    metamodelManager.SetAudit(metaEntity.Name, metaEntity.EntityId, this.StrToAuditType(metaEntity.AuditMode));
                }
            }
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
            object referenceObject = null;
            if (actionType == TransacActionType.Insert)
            {
                referenceObject = target;
            }
            else
            {
                referenceObject = source;
            }
            IMetamodelEntity metamodelEntity = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(referenceObject.GetType());
            return ((metamodelEntity != null) && (metamodelEntity.AuditType != AuditType.None));
        }

        /// <summary>
        /// Commits the transaction information.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="transac">The transaction information.</param>
        protected override void CommitTransac(IUserContext userContext, TransacInfo transac)
        {
            if (transac.Actions.Count > 0)
            {
                AuditTransac transacEntity = new AuditTransac();
                transacEntity.UserId = Convert.ToInt32(transac.UserId);
                transacEntity.StartDttm = transac.StartDateTime;
                transacEntity.EndDttm = transac.EndDateTime;
                userContext.Context.Add(transacEntity);
                userContext.Context.FlushChanges();
                transac.TransacId = transacEntity.AuditTransacId;
            }
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
            object referenceObject = null;
            if (action.Type == TransacActionType.Insert)
            {
                referenceObject = action.TargetInstance;
            }
            else
            {
                referenceObject = action.SourceInstance;
            }
            IMetamodelEntity metamodelEntity = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(referenceObject.GetType());
            if (metamodelEntity != null)
            {
                if (metamodelEntity.AuditType == AuditType.Simple)
                {
                    AuditTransacAction actionEntity = new AuditTransacAction();
                    actionEntity.AuditTransacId = Convert.ToInt32(transac.TransacId);
                    actionEntity.AuditTransacActionIx = index + 1;
                    actionEntity.EntityId = metamodelEntity.EntId;
                    actionEntity.PrimaryKey = metamodelEntity.GetStringKey(referenceObject);
                    userContext.Context.Add(actionEntity);
                }
            }
        }

        #endregion

        #endregion
    }
}