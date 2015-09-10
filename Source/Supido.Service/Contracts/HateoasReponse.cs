
namespace Supido.Service.Contracts
{
    /// <summary>
    /// Class for enveloping a list of results and add the links of the list.
    /// </summary>
    public class HateoasReponse
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public object Items { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public object Links { get; set; }

        #endregion
    }
}
