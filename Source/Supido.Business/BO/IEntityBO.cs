using System;

namespace Supido.Business.BO
{
    /// <summary>
    /// Interface for a BO that exposes an Entity type.
    /// </summary>
    public interface IEntityBO
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        Type EntityType { get; set; }

        /// <summary>
        /// Gets the type of the dto.
        /// </summary>
        /// <value>
        /// The type of the dto.
        /// </value>
        Type DtoType { get; }

        #endregion
    }
}
