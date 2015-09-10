using System;
using System.Collections.Generic;

namespace Supido.Business.Audit
{
    /// <summary>
    /// Class for the information of a transaction
    /// </summary>
    public class TransacInfo
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public long? TransacId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the start date time.
        /// </summary>
        /// <value>
        /// The start date time.
        /// </value>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time.
        /// </value>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        public IList<TransacActionInfo> Actions { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TransacInfo"/> class.
        /// </summary>
        public TransacInfo()
        {
            this.Actions = new List<TransacActionInfo>();
        }

        #endregion
    }
}
