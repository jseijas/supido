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
	public partial class AuditTransac
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
		
		private int userId;
		public virtual int UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.userId = value;
			}
		}
		
		private DateTime startDttm;
		public virtual DateTime StartDttm
		{
			get
			{
				return this.startDttm;
			}
			set
			{
				this.startDttm = value;
			}
		}
		
		private DateTime? endDttm;
		public virtual DateTime? EndDttm
		{
			get
			{
				return this.endDttm;
			}
			set
			{
				this.endDttm = value;
			}
		}
		
		private User user;
		public virtual User User
		{
			get
			{
				return this.user;
			}
			set
			{
				this.user = value;
			}
		}
		
		private IList<AuditTransacAction> auditTransacActions = new List<AuditTransacAction>();
		public virtual IList<AuditTransacAction> AuditTransacActions
		{
			get
			{
				return this.auditTransacActions;
			}
		}
		
	}
}
#pragma warning restore 1591
