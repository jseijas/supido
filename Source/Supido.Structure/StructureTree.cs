using Supido.Core.Utils;
using Supido.Structure.Meta;
using System.Collections.Generic;

namespace Supido.Structure
{
    public class StructureTree
    {
        #region - Fields -

        private Dictionary<string, StructureItem> itemMap = new Dictionary<string, StructureItem>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the domains.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        public IList<StructureDomain> Domains { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public IList<StructureEntity> Entities { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureTree"/> class.
        /// </summary>
        public StructureTree()
        {
            this.Domains = new List<StructureDomain>();
            this.Entities = new List<StructureEntity>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddItem(StructureItem item)
        {
            if (item is StructureDomain)
            {
                this.Domains.Add((StructureDomain)item);
            }
            else
            {
                this.Entities.Add((StructureEntity)item);
            }
        }

        private string GenerateDomainName(DataType type, int length, int decimals, bool isIdentity)
        {
            string result = "Generated";
            if (isIdentity) 
            {
                result = result+"Identity";
            }
            switch (type)
            {
                case DataType.Tinyint: return result + "Tinyint";
                case DataType.Smallint: return result + "Smallint";
                case DataType.Int: return result + "Int";
                case DataType.Bigint: return result + "Bigint";
                case DataType.Float: return result + "Float";
                case DataType.Date: return result + "Date";
                case DataType.DateTime: return result + "DateTime";
                case DataType.Money: return result + "Money";
                case DataType.Boolean: return result + "Boolean";
                case DataType.Numeric:
                    if (length == 0)
                    {
                        length = 18;
                    }
                    if (decimals == 0)
                    {
                        decimals = 2;
                    }
                    return result + "Numeric_" + length.ToString() + "_" + decimals.ToString();
                case DataType.String:
                    if (length == 0)
                    {
                        return result + "String";
                    }
                    return result + "String_" + length.ToString();
                default: return result;


            }
        }

        /// <summary>
        /// Builds the metamodel.
        /// </summary>
        /// <returns></returns>
        public MetaModel Build()
        {
            MetaModel result = new MetaModel();
            foreach (StructureDomain domain in this.Domains)
            {
                MetaDomain metaDomain = new MetaDomain();
                metaDomain.Id = null;
                metaDomain.Name = domain.Name;
                metaDomain.IsIdentity = domain.IsIdentity;
                metaDomain.DataType = domain.DataType;
                metaDomain.DataTypeLength = domain.DataTypeLength;
                metaDomain.DataTypeDecimals = domain.DataTypeDecimals;
                metaDomain.IsUsed = false;
                result.AddDomain(metaDomain);
            }
            foreach (StructureEntity entity in this.Entities)
            {
                MetaTable table = new MetaTable();
                table.Id = null;
                table.Name = entity.Name;
                table.PhysicalName = StringUtil.Camelize(table.Name);
                table.LogicalName = StringUtil.Pascalize(table.Name);
                result.AddTable(table);
                foreach (StructureProperty property in entity.Properties)
                {
                    if ((property.ReferenceItem == null) || (property.ReferenceItem is StructureDomain))
                    {
                        MetaColumn column = new MetaColumn();
                        column.TableId = null;
                        column.Ix = null;
                        column.Name = property.Name;
                        MetaDomain metaDomain;
                        if (property.ReferenceItem == null)
                        {
                            string generatedDomainName = this.GenerateDomainName(property.DataType, property.DataTypeLength, property.DataTypeDecimals, property.IsIdentity);
                            metaDomain = result.GetDomain(generatedDomainName);
                            if (metaDomain == null)
                            {
                                metaDomain = new MetaDomain();
                                metaDomain.Id = null;
                                metaDomain.Name = generatedDomainName;
                                metaDomain.IsIdentity = property.IsIdentity;
                                metaDomain.DataType = property.DataType;
                                metaDomain.DataTypeLength = property.DataTypeLength;
                                metaDomain.DataTypeDecimals = property.DataTypeDecimals;
                                result.AddDomain(metaDomain);
                            }
                        }
                        else
                        {
                            metaDomain = result.GetDomain(property.ReferenceItem.Name);                            
                        }
                        metaDomain.IsUsed = true;
                        column.Domain = metaDomain;
                        column.IsIdentity = property.IsIdentity || metaDomain.IsIdentity;
                        column.IsPrimaryKey = property.IsPrimaryKey || column.IsPrimaryKey;
                        column.IsNullable = property.IsNullable && !column.IsPrimaryKey;
                        column.DataType = metaDomain.DataType;
                        column.DataTypeLength = metaDomain.DataTypeLength;
                        column.DataTypeDecimals = metaDomain.DataTypeLength;
                        column.IsForeignKey = false;
                        table.AddColumn(column);
                    }
                    else
                    {
                        MetaRelation relation = new MetaRelation();
                        relation.ParentName = property.ReferenceItem.Name;
                        relation.SonRelationName = property.Name;
                        relation.Son = table;
                        relation.IsKeyInSon = property.IsPrimaryKey;
                        relation.IsNullableInSon = property.IsNullable;
                        result.Relations.Add(relation);
                    }
                }
            }
            result.Build();
            return result;
        }

        #endregion
    }
}
