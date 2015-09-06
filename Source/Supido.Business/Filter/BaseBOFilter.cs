using Supido.Business.BO;
using Supido.Business.Query;
using System.Linq;

namespace Supido.Business.Filter
{
    /// <summary>
    /// Abstract class for a base business object filter
    /// </summary>
    public abstract class BaseBOFilter : IBOFilter
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public IEntityBO Parent { get; set; }

        #endregion

        #region - Methods -

        #region - Protected Abstract Methods -

        /// <summary>
        /// Applies the order filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        protected abstract IQueryable ApplyOrderFilter(IQueryable query, QueryInfo info);

        /// <summary>
        /// Applies the where filter.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        protected abstract IQueryable ApplyWhereFilter(IQueryable query, QueryInfo info);

        #endregion

        #region - Methods from IBOFilter -

        /// <summary>
        /// Applies the security filter to the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public abstract IQueryable ApplySecurity(IQueryable query);

        /// <summary>
        /// Applies the query info filter to the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        public IQueryable ApplyFilter(IQueryable query, Query.QueryInfo info)
        {
            IQueryable result = query;
            result = this.ApplyWhereFilter(result, info);
            result = this.ApplyOrderFilter(result, info);
            return result;
        }

        #endregion

        #endregion

    }
}
