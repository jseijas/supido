using System.Collections.Generic;

namespace Supido.Structure
{
    /// <summary>
    /// Class for an structure entity
    /// </summary>
    public class StructureEntity : StructureItem
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IList<StructureProperty> Properties { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureEntity"/> class.
        /// </summary>
        public StructureEntity()
        {
            this.Properties = new List<StructureProperty>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureEntity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public StructureEntity(string name)
            : this()
        {
            this.Name = name;
        }

        #endregion
    }
}
