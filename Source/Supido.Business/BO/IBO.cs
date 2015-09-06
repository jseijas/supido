using Supido.Business.Query;
using System.Collections.Generic;

namespace Supido.Business.BO
{
    /// <summary>
    /// Interface for a business object based on a DTO.
    /// </summary>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public interface IBO<TDto>
    {
        #region - Methods -

        /// <summary>
        /// Gets all the DTOs.
        /// </summary>
        /// <returns></returns>
        IList<TDto> GetAll();

        /// <summary>
        /// Gets all the DTOs applying query information.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        IList<TDto> GetAll(QueryInfo info);

        /// <summary>
        /// Gets one DTO.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        TDto GetOne(string pk);

        /// <summary>
        /// Gets one DTO filtering also by query information.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        TDto GetOne(string pk, QueryInfo info);

        /// <summary>
        /// Inserts one DTO.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        TDto Insert(TDto src);

        /// <summary>
        /// Updates one DTO.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        TDto Update(TDto src);

        /// <summary>
        /// Deletes one DTO by primary key.
        /// </summary>
        /// <param name="pk">The primary key.</param>
        /// <returns></returns>
        int Delete(string pk);

        #endregion
    }
}
