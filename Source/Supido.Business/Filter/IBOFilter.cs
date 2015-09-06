using Supido.Business.BO;
using Supido.Business.Query;
using System.Linq;

namespace Supido.Business.Filter
{
    /// <summary>
    /// Interface for the filter of a Business Object.
    /// </summary>
    public interface IBOFilter
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        IEntityBO Parent { get; set; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Applies the security filter to the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IQueryable ApplySecurity(IQueryable query);

        /// <summary>
        /// Applies the query info filter to the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        IQueryable ApplyFilter(IQueryable query, QueryInfo info);

        #endregion
    }
}
