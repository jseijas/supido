using System.Collections.Generic;

namespace Supido.Structure
{
    /// <summary>
    /// Class for a domain.
    /// </summary>
    public class StructureDomain : StructureItem
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets the data type as string.
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
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public DataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the aliases.
        /// </summary>
        /// <value>
        /// The aliases.
        /// </value>
        public IList<string> Aliases { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureDomain"/> class.
        /// </summary>
        public StructureDomain()
        {
            this.Aliases = new List<string>();
            this.IsIdentity = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureDomain"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public StructureDomain(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion
    }
}
