using Supido.Business;
using Supido.Business.Query;
using Supido.Core.Container;
using Supido.Service.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Service.Contracts
{
    public class MessagePath
    {
        public MessageInformation Information { get; set; }

        public IServiceConfiguration Configuration { get; set; }

        public ISecurityManager SecurityManager { get; set; }

        public bool HasKeyParameter { get; set; }

        public string KeyParameter { get; set; }

        public QueryInfo QueryInfo { get; set; }

        public ApiNode CurrentNode { get; set; }

        public Type EntityType { get; set; }

        public Type DtoType { get; set; }

        public bool IsQuery { get; set; }

        public bool IsByParent { get; set; }

        public string ParentKeyParameter { get; set; }

        public string KeyParameterName { get; set; }

        public HateoasList Links { get; set; }

        public MessagePath(MessageInformation information)
        {
            this.Configuration = IoC.Get<IServiceConfiguration>();
            this.SecurityManager = IoC.Get<ISecurityManager>();
            this.Information = information;
            this.Build(false);
        }

        public MessagePath(MessageInformation information, bool forParent)
        {
            this.Configuration = IoC.Get<IServiceConfiguration>();
            this.SecurityManager = IoC.Get<ISecurityManager>();
            this.Information = information;
            this.Build(forParent);
        }

        private string BuildParentLink()
        {
            string result = this.Information.AbsoluteApiPath;
            for (int i = 0; i < this.Information.PathTokens.Count-1; i++)
            {
                result = result + "/" + this.Information.PathTokens[i];
            }
            return result+"?sessionToken="+this.Information.LowParameters["sessiontoken"];
        }

        private string BuildSonLink(string path)
        {
            string result = this.Information.AbsoluteApiPath;
            for (int i = 0; i < this.Information.PathTokens.Count - 1; i++)
            {
                result = result + "/" + this.Information.PathTokens[i];
            }
            return result + "/"+path+"?sessionToken=" + this.Information.LowParameters["sessiontoken"];
        }

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

    }
}
