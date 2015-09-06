using System.Collections.Generic;

namespace Supido.Business.Query
{
    /// <summary>
    /// Class for the information of a facet.
    /// </summary>
    public class FacetInfo
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the values of the facet.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IList<FacetValueInfo> Values { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetInfo"/> class.
        /// </summary>
        public FacetInfo()
        {
            this.Values = new List<FacetValueInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FacetInfo(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public FacetInfo(string name, IList<FacetValueInfo> values)
            : this(name)
        {
            foreach (FacetValueInfo value in values)
            {
                this.Values.Add(value);
            }
        }

        #endregion

    }
}
