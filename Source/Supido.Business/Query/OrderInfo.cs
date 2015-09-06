
namespace Supido.Business.Query
{
    /// <summary>
    /// Class for the information of the order by a field.
    /// </summary>
    public class OrderInfo
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
        /// Gets or sets a value indicating whether this instance is ascending.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ascending; otherwise, <c>false</c>.
        /// </value>
        public bool IsAscending { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderInfo"/> class.
        /// </summary>
        public OrderInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public OrderInfo(string name)
        {
            this.Name = name;
            this.IsAscending = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isAscending">if set to <c>true</c> [is ascending].</param>
        public OrderInfo(string name, bool isAscending)
        {
            this.Name = name;
            this.IsAscending = isAscending;
        }

        #endregion
    }
}
