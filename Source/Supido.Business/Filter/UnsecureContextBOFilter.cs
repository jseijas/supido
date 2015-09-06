using System.Linq;

namespace Supido.Business.Filter
{
    /// <summary>
    /// Unsecure filter for a BO context based.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class UnsecureContextBOFilter<TEntity> : BaseContextBOFilter<TEntity>
    {
        /// <summary>
        /// Applies the security filter to the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public override IQueryable ApplySecurity(IQueryable query)
        {
            return query;
        }
    }

}
