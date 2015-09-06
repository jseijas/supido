
namespace Supido.Business.Meta
{
    /// <summary>
    /// Class for a metamodel field.
    /// </summary>
    public class MetamodelField : IMetamodelField
    {
        #region - Properties -

        /// <summary>
        /// Gets the entity name of the field.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether [avoid update].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [avoid update]; otherwise, <c>false</c>.
        /// </value>
        public bool AvoidUpdate { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetamodelField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isPrimaryKey">if set to <c>true</c> [is primary key].</param>
        /// <param name="avoidUpdate">if set to <c>true</c> [avoid update].</param>
        public MetamodelField(string name, bool isPrimaryKey, bool avoidUpdate)
        {
            this.Name = name;
            this.IsPrimaryKey = isPrimaryKey;
            this.AvoidUpdate = avoidUpdate;
        }

        #endregion
    }
}
