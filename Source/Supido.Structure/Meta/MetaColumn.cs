
namespace Supido.Structure.Meta
{
    /// <summary>
    /// Class for a column of the database
    /// </summary>
    public class MetaColumn
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public MetaTable Table { get; set; }

        /// <summary>
        /// Gets or sets the table identifier.
        /// </summary>
        /// <value>
        /// The table identifier.
        /// </value>
        public long? TableId { get; set; }

        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        /// <value>
        /// The ix.
        /// </value>
        public long? Ix { get; set; }

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
        /// The name of the logical.
        /// </value>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the physical name
        /// </summary>
        /// <value>
        /// The name of the physical.
        /// </value>
        public string PhysicalName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public DataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the length of the data type.
        /// </summary>
        /// <value>
        /// The length of the data type.
        /// </value>
        public int DataTypeLength { get; set; }

        /// <summary>
        /// Gets or sets the data type decimals.
        /// </summary>
        /// <value>
        /// The data type decimals.
        /// </value>
        public int DataTypeDecimals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is foreign key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
        /// </value>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the target column.
        /// </summary>
        /// <value>
        /// The target column.
        /// </value>
        public MetaColumn TargetColumn { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public MetaDomain Domain { get; set; }

        #endregion
    }
}
