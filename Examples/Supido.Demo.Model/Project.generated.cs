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
	public partial class Project
	{
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
		
		private int clientId;
		public virtual int ClientId
		{
			get
			{
				return this.clientId;
			}
			set
			{
				this.clientId = value;
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
		
		private Client client;
		public virtual Client Client
		{
			get
			{
				return this.client;
			}
			set
			{
				this.client = value;
			}
		}
		
		private IList<Service> services = new List<Service>();
		public virtual IList<Service> Services
		{
			get
			{
				return this.services;
			}
		}
		
	}
}
#pragma warning restore 1591
