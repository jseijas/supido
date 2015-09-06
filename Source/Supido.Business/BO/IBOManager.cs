using System;

namespace Supido.Business.BO
{
    /// <summary>
    /// Interface for a manager of Business Objects
    /// </summary>
    public interface IBOManager
    {
        #region - Methods -

        /// <summary>
        /// Adds a filter type related to a DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="filterType">Type of the filter.</param>
        void AddFilterType(Type dtoType, Type filterType);

        /// <summary>
        /// Adds a filter type related to a DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="filterType">Type of the filter.</param>
        void AddFilterType<TDto>(Type filterType);

        /// <summary>
        /// Adds a BO type related to a DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="contextBOType">Type of the context bo.</param>
        void AddBOType(Type dtoType, Type contextBOType);

        /// <summary>
        /// Adds a BO type related to a DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="contextBOType">Type of the context bo.</param>
        void AddBOType<TDto>(Type contextBOType);

        /// <summary>
        /// Gets the type of the BO from the DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        Type GetBOType<TDto>();

        /// <summary>
        /// Gets the type of the filter from the DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        Type GetFilterType<TDto>();

        #endregion

    }
}
