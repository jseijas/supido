
namespace Supido.Business.Audit
{
    /// <summary>
    /// Class for an action inside a transaction.
    /// </summary>
    public class TransacActionInfo
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public TransacActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the source instance.
        /// </summary>
        /// <value>
        /// The source instance.
        /// </value>
        public object SourceInstance { get; set; }

        /// <summary>
        /// Gets or sets the target instance.
        /// </summary>
        /// <value>
        /// The target instance.
        /// </value>
        public object TargetInstance { get; set; }

        #endregion
    }
}
