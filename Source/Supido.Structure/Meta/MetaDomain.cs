
namespace Supido.Structure.Meta
{
    /// <summary>
    /// Class for a domain for the metamodel.
    /// </summary>
    public class MetaDomain
    {
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
        /// Gets or sets a value indicating whether this instance is used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is used; otherwise, <c>false</c>.
        /// </value>
        public bool IsUsed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaDomain"/> class.
        /// </summary>
        public MetaDomain()
        {
        }
    }
}
