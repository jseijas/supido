#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using Supido.Demo.Model;

namespace Supido.Demo.Model	
{
	public partial class AuditTransacAction
	{
		private long auditTransacId;
		public virtual long AuditTransacId
		{
			get
			{
				return this.auditTransacId;
			}
			set
			{
				this.auditTransacId = value;
			}
		}
		
		private int auditTransacActionIx;
		public virtual int AuditTransacActionIx
		{
			get
			{
				return this.auditTransacActionIx;
			}
			set
			{
				this.auditTransacActionIx = value;
			}
		}
		
		private int entityId;
		public virtual int EntityId
		{
			get
			{
				return this.entityId;
			}
			set
			{
				this.entityId = value;
			}
		}
		
		private Char actionType;
		public virtual Char ActionType
		{
			get
			{
				return this.actionType;
			}
			set
			{
				this.actionType = value;
			}
		}
		
		private string primaryKey;
		public virtual string PrimaryKey
		{
			get
			{
				return this.primaryKey;
			}
			set
			{
				this.primaryKey = value;
			}
		}
		
		private string srcObject;
		public virtual string SrcObject
		{
			get
			{
				return this.srcObject;
			}
			set
			{
				this.srcObject = value;
			}
		}
		
		private string tgtObject;
		public virtual string TgtObject
		{
			get
			{
				return this.tgtObject;
			}
			set
			{
				this.tgtObject = value;
			}
		}
		
		private MetaEntity metaEntity;
		public virtual MetaEntity MetaEntity
		{
			get
			{
				return this.metaEntity;
			}
			set
			{
				this.metaEntity = value;
			}
		}
		
		private AuditTransac auditTransac;
		public virtual AuditTransac AuditTransac
		{
			get
			{
				return this.auditTransac;
			}
			set
			{
				this.auditTransac = value;
			}
		}
		
	}
}
#pragma warning restore 1591
