using System.Collections.Generic;

namespace Supido.Service.Contracts
{
    /// <summary>
    /// Class representing a list of HATEOAS links.
    /// </summary>
    public class HateoasList : List<HateoasLink>
    {
        #region - Methods -

        /// <summary>
        /// Adds a new HATEOAS link.
        /// </summary>
        /// <param name="rel">The relative.</param>
        /// <param name="href">The href.</param>
        /// <returns></returns>
        public HateoasLink Add(string rel, string href) 
        {
            HateoasLink result = new HateoasLink(rel, href);
            this.Add(result);
            return result;
        }

        #endregion
    }
}
