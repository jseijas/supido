using System.Collections.Generic;

namespace Supido.Service.Contracts
{
    public class HateoasList : List<HateoasLink>
    {
        public HateoasLink Add(string rel, string href) 
        {
            HateoasLink result = new HateoasLink(rel, href);
            this.Add(result);
            return result;
        }
    }
}
