using Supido.Core.Utils;
using System.Collections.Generic;

namespace Supido.Structure.Meta
{
    /// <summary>
    /// Class for the metamodel of a database
    /// </summary>
    public class MetaModel
    {
        #region - Fields -

        private Dictionary<string, MetaDomain> domainMap = new Dictionary<string, MetaDomain>();

        private Dictionary<string, MetaTable> tableMap = new Dictionary<string, MetaTable>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the tables.
        /// </summary>
        /// <value>
        /// The tables.
        /// </value>
        public IList<MetaTable> Tables { get; set; }

        /// <summary>
        /// Gets or sets the domains.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        public IList<MetaDomain> Domains { get; set; }

        /// <summary>
        /// Gets or sets the relations.
        /// </summary>
        /// <value>
        /// The relations.
        /// </value>
        public IList<MetaRelation> Relations { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaModel"/> class.
        /// </summary>
        public MetaModel()
        {
            this.Domains = new List<MetaDomain>();
            this.Tables = new List<MetaTable>();
            this.Relations = new List<MetaRelation>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public MetaTable GetTable(string name)
        {
            if (this.tableMap.ContainsKey(name.ToLower()))
            {
                return this.tableMap[name.ToLower()];
            }
            return null;
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public MetaDomain GetDomain(string name)
        {
            if (this.domainMap.ContainsKey(name.ToLower()))
            {
                return this.domainMap[name.ToLower()];
            }
            return null;
        }

        /// <summary>
        /// Adds the table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void AddTable(MetaTable table)
        {
            this.Tables.Add(table);
            this.tableMap.Add(table.Name.ToLower(), table);
        }

        /// <summary>
        /// Adds the domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public void AddDomain(MetaDomain domain)
        {
            this.Domains.Add(domain);
            this.domainMap.Add(domain.Name.ToLower(), domain);
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public void Build()
        {
            foreach (MetaRelation relation in this.Relations)
            {
                MetaTable parentTable = this.GetTable(relation.ParentName);
                relation.Parent = parentTable;
                IList<MetaColumn> parentKeys = parentTable.GetPrimaryKeys();
                foreach (MetaColumn key in parentKeys)
                {
                    MetaColumn column = new MetaColumn();
                    if (key.Name.ToLower().StartsWith(relation.SonRelationName.ToLower()))
                    {
                        column.Name = key.Name;
                    }
                    else
                    {
                        column.Name = relation.SonRelationName + StringUtil.FirstUpper(key.Name);
                    }
                    column.IsIdentity = false;
                    column.IsPrimaryKey = relation.IsKeyInSon;
                    column.IsNullable = relation.IsNullableInSon;
                    column.DataType = key.DataType;
                    column.DataTypeLength = key.DataTypeLength;
                    column.DataTypeDecimals = key.DataTypeDecimals;
                    column.IsForeignKey = true;
                    column.TargetColumn = key;
                    relation.Son.AddColumn(column);
                    relation.ParentColumns.Add(key);
                    relation.SonColumns.Add(column);
                }
            }
            IList<MetaDomain> unusedDomains = new List<MetaDomain>();
            foreach (MetaDomain domain in this.Domains)
            {
                if (!domain.IsUsed)
                {
                    unusedDomains.Add(domain);
                }
            }
            foreach (MetaDomain domain in unusedDomains)
            {
                this.domainMap.Remove(domain.Name);
                this.Domains.Remove(domain);
            }
            int i = 1;
            foreach (MetaDomain domain in this.Domains)
            {
                domain.Id = i;
                i++;
            }
            i = 1;
            foreach (MetaTable table in this.Tables)
            {
                table.Id = i;
                int j = 1;
                foreach (MetaColumn column in table.Columns)
                {
                    column.TableId = table.Id;
                    column.Ix = j;
                    j++;
                }
                i++;
            }
        }

        #endregion
    }
}
