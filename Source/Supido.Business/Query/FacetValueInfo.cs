
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Supido.Business.Query
{
    /// <summary>
    /// Class for one facet value
    /// </summary>
    public class FacetValueInfo
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public FacetOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetValueInfo"/> class.
        /// </summary>
        public FacetValueInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetValueInfo"/> class.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        public FacetValueInfo(FacetOperation operation, string value)
        {
            this.Operation = operation;
            this.Value = value;
        }

        #endregion
    }
}
