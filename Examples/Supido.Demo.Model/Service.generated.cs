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
	public partial class Service
	{
		private int serviceId;
		public virtual int ServiceId
		{
			get
			{
				return this.serviceId;
			}
			set
			{
				this.serviceId = value;
			}
		}
		
		private int projectId;
		public virtual int ProjectId
		{
			get
			{
				return this.projectId;
			}
			set
			{
				this.projectId = value;
			}
		}
		
		private string name;
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		
		private Project project;
		public virtual Project Project
		{
			get
			{
				return this.project;
			}
			set
			{
				this.project = value;
			}
		}
		
		private IList<Task> tasks = new List<Task>();
		public virtual IList<Task> Tasks
		{
			get
			{
				return this.tasks;
			}
		}
		
	}
}
#pragma warning restore 1591
