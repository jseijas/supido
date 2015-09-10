
namespace Supido.Service.Contracts
{
    /// <summary>
    /// Class representing a HATEOAS link.
    /// </summary>
    public class HateoasLink
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the relative.
        /// </summary>
        /// <value>
        /// The relative.
        /// </value>
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        public string Href { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="HateoasLink"/> class.
        /// </summary>
        public HateoasLink()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HateoasLink"/> class.
        /// </summary>
        /// <param name="rel">The relative.</param>
        /// <param name="href">The href.</param>
        public HateoasLink(string rel, string href)
        {
            this.Rel = rel;
            this.Href = href;
        }

        #endregion
    }
}
