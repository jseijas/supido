using Supido.Business;
using Supido.Business.Audit;
using Supido.Business.Context;
using Supido.Business.Meta;
using Supido.Core.Container;
using Supido.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supido.Demo.Service.Security
{
    public class AuditManager : BaseAuditManager
    {
        private AuditType StrToAuditType(string str)
        {
            str = str.ToLower();
            if (str.Equals("simple"))
            {
                return AuditType.Simple;
            }
            else if (str.Equals("full"))
            {
                return AuditType.Full;
            }
            else
            {
                return AuditType.None;
            }
        }

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
    }
}