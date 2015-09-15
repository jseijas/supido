
namespace Supido.Structure
{
    /// <summary>
    /// Class for a property
    /// </summary>
    public class StructureProperty
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the reference.
        /// </summary>
        /// <value>
        /// The name of the reference.
        /// </value>
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is typed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is typed; otherwise, <c>false</c>.
        /// </value>
        public bool IsTyped { get; set; }

        /// <summary>
        /// Gets or sets the reference item.
        /// </summary>
        /// <value>
        /// The reference item.
        /// </value>
        public StructureItem ReferenceItem { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public DataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the data type string.
        /// </summary>
        /// <value>
        /// The data type string.
        /// </value>
        public string DataTypeStr { get; set; }

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
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureProperty"/> class.
        /// </summary>
        public StructureProperty()
        {
            this.IsIdentity = false;
            this.IsPrimaryKey = false;
            this.IsNullable = false;
            this.IsTyped = false;
        }

        #endregion
    }
}
