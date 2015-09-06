
namespace Supido.Business.Meta
{
    /// <summary>
    /// Interface for a field of the metamodel
    /// </summary>
    public interface IMetamodelField
    {
        #region - Properties -

        /// <summary>
        /// Gets the entity name of the field.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        bool IsPrimaryKey { get; }

        /// <summary>
        /// Gets a value indicating whether [avoid update].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [avoid update]; otherwise, <c>false</c>.
        /// </value>
        bool AvoidUpdate { get; }

        #endregion
    }
}
