
namespace Supido.Service.Contracts
{
    public class HateoasLink
    {
        public string Rel { get; set; }

        public string Href { get; set; }

        public HateoasLink()
        {

        }

        public HateoasLink(string rel, string href)
        {
            this.Rel = rel;
            this.Href = href;
        }

    }
}
