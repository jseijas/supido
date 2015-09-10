using Supido.Business;
using Supido.Business.Query;
using Supido.Core.Container;
using Supido.Service.Configuration;
using System;
using System.Collections.Generic;

namespace Supido.Service.Contracts
{
    /// <summary>
    /// Class for a Message Path, that has the information of the message and fron the URI and API builds all the structure.
    /// </summary>
    public class MessagePath
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public MessageInformation Information { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IServiceConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the security manager.
        /// </summary>
        /// <value>
        /// The security manager.
        /// </value>
        public ISecurityManager SecurityManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has key parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has key parameter; otherwise, <c>false</c>.
        /// </value>
        public bool HasKeyParameter { get; set; }

        /// <summary>
        /// Gets or sets the key parameter.
        /// </summary>
        /// <value>
        /// The key parameter.
        /// </value>
        public string KeyParameter { get; set; }

        /// <summary>
        /// Gets or sets the query information.
        /// </summary>
        /// <value>
        /// The query information.
        /// </value>
        public QueryInfo QueryInfo { get; set; }

        /// <summary>
        /// Gets or sets the current API node.
        /// </summary>
        /// <value>
        /// The current node.
        /// </value>
        public ApiNode CurrentNode { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the dto.
        /// </summary>
        /// <value>
        /// The type of the dto.
        /// </value>
        public Type DtoType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is query.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is query; otherwise, <c>false</c>.
        /// </value>
        public bool IsQuery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is by parent.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is by parent; otherwise, <c>false</c>.
        /// </value>
        public bool IsByParent { get; set; }

        /// <summary>
        /// Gets or sets the parent key parameter.
        /// </summary>
        /// <value>
        /// The parent key parameter.
        /// </value>
        public string ParentKeyParameter { get; set; }

        /// <summary>
        /// Gets or sets the name of the key parameter.
        /// </summary>
        /// <value>
        /// The name of the key parameter.
        /// </value>
        public string KeyParameterName { get; set; }

        /// <summary>
        /// Gets or sets the HATEOAS links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public HateoasList Links { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePath"/> class.
        /// </summary>
        /// <param name="information">The information.</param>
        public MessagePath(MessageInformation information)
        {
            this.Configuration = IoC.Get<IServiceConfiguration>();
            this.SecurityManager = IoC.Get<ISecurityManager>();
            this.Information = information;
            this.Build(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePath"/> class.
        /// </summary>
        /// <param name="information">The information.</param>
        /// <param name="forParent">if set to <c>true</c> [for parent].</param>
        public MessagePath(MessageInformation information, bool forParent)
        {
            this.Configuration = IoC.Get<IServiceConfiguration>();
            this.SecurityManager = IoC.Get<ISecurityManager>();
            this.Information = information;
            this.Build(forParent);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Builds the parent link.
        /// </summary>
        /// <returns></returns>
        private string BuildParentLink()
        {
            string result = this.Information.AbsoluteApiPath;
            for (int i = 0; i < this.Information.PathTokens.Count-1; i++)
            {
                result = result + "/" + this.Information.PathTokens[i];
            }
            return result+"?sessionToken="+this.Information.LowParameters["sessiontoken"];
        }

        /// <summary>
        /// Builds the son link.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string BuildSonLink(string path)
        {
            string result = this.Information.AbsoluteApiPath;
            for (int i = 0; i < this.Information.PathTokens.Count - 1; i++)
            {
                result = result + "/" + this.Information.PathTokens[i];
            }
            return result + "/"+path+"?sessionToken=" + this.Information.LowParameters["sessiontoken"];
        }

        /// <summary>
        /// Builds the links.
        /// </summary>
        private void BuildLinks()
        {
            this.Links.Add("self", this.Information.AbsoluteUri);
            this.Links.Add("parent", this.BuildParentLink());
            if (this.HasKeyParameter)
            {
                foreach (KeyValuePair<string, ApiNode> kvp in this.CurrentNode.Sons)
                {
                    this.Links.Add(kvp.Value.Path, this.BuildSonLink(kvp.Value.Path));
                }
            }
        }

        /// <summary>
        /// Builds the specified for parent.
        /// </summary>
        /// <param name="forParent">if set to <c>true</c> [for parent].</param>
        /// <exception cref="System.ArgumentException"></exception>
        private void Build(bool forParent)
        {
            this.Links = new HateoasList();
            if (this.Information.PathTokens[this.Information.PathTokens.Count-1].ToLower().Equals("query")) 
            {
                this.IsQuery = true;
            }
            else
            {
                this.IsQuery = false;
            }
            ApiNode node = Configuration.Root;
            IList<string> paths = new List<string>();
            IList<string> values = new List<string>();
            int max = this.IsQuery ? this.Information.PathTokens.Count - 1 : this.Information.PathTokens.Count;
            if (forParent)
            {
                max = max - 1;
            }
            this.HasKeyParameter = max % 2 == 0;
            Type entityType = null;
            for (int i = 0; i < max; i++)
            {
                if ((i % 2) == 0)
                {
                    if (node.Sons.ContainsKey(this.Information.PathTokens[i]))
                    {
                        node = node.Sons[Information.PathTokens[i]];
                        entityType = this.SecurityManager.MetamodelManager.GetEntityByDto(node.DtoType).EntityType;
                        for (int j = 0; j < paths.Count; j++) 
                        {
                            paths[j] = string.Format("{0}.{1}", entityType.Name, paths[j]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Api path not found: {0}", this.Information.Path));
                    }
                }
                else
                {
                    paths.Add(node.ParameterName);
                    values.Add(this.Information.PathTokens[i]);
                }
            }
            if (this.HasKeyParameter)
            {
                this.KeyParameter = values[values.Count - 1];
            }
            else
            {
                this.KeyParameter = string.Empty;
            }
            this.CurrentNode = node;
            if (!string.IsNullOrEmpty(node.ParentParameterName))
            {
                this.IsByParent = true;
                this.ParentKeyParameter = node.ParentParameterName;
            }
            else
            {
                this.IsByParent = false;
                this.ParentKeyParameter = string.Empty;
            }
            this.KeyParameterName = node.ParameterName;
            this.EntityType = entityType;
            this.DtoType = node.DtoType;
            this.BuildLinks();
            max = this.HasKeyParameter ? paths.Count-1 : paths.Count;
            for (int i = 0; i < max; i++)
            {
                paths[i] = paths[i].Substring(entityType.Name.Length + 1);
            }
            this.QueryInfo = new QueryInfo();
            for (int i = 0; i < paths.Count; i++)
            {
                this.QueryInfo.Equal(paths[i], values[i]);
            }
        }

        #endregion
    }
}
