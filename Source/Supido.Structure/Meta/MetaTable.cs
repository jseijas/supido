using System.Collections.Generic;

namespace Supido.Structure.Meta
{
    /// <summary>
    /// Class for a table of the model.
    /// </summary>
    public class MetaTable
    {
        #region - Fields -

        private Dictionary<string, MetaColumn> columnMap = new Dictionary<string, MetaColumn>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the logical name.
        /// </summary>
        /// <value>
        /// The logical name.
        /// </value>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the physical name.
        /// </summary>
        /// <value>
        /// The physical name.
        /// </value>
        public string PhysicalName { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public IList<MetaColumn> Columns { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaTable"/> class.
        /// </summary>
        public MetaTable()
        {
            this.Columns = new List<MetaColumn>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public MetaColumn GetColumn(string name)
        {
            if (this.columnMap.ContainsKey(name.ToLower()))
            {
                return this.columnMap[name.ToLower()];
            }
            return null;
        }

        /// <summary>
        /// Gets the primary keys.
        /// </summary>
        /// <returns></returns>
        public IList<MetaColumn> GetPrimaryKeys()
        {
            IList<MetaColumn> result = new List<MetaColumn>();
            foreach (MetaColumn column in this.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    result.Add(column);
                }
            }
            return result;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void AddColumn(MetaColumn column)
        {
            column.Table = this;
            this.Columns.Add(column);
            this.columnMap.Add(column.Name.ToLower(), column);
        }

        #endregion

    }
}
