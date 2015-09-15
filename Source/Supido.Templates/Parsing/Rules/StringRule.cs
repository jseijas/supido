
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for a string.
    /// </summary>
    public class StringRule : Rule
    {
        #region - Fields -

        /// <summary>
        /// The value
        /// </summary>
        private string value;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StringRule"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringRule(string value)
            : base()
        {
            this.value = value;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Indicates if the current parser state matches the rule.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public override bool Match(ParserState state)
        {
            if (!state.CurrentInput.StartsWith(this.value))
            {
                return false;
            }
            state.CurrentPosition = state.CurrentPosition + this.value.Length;
            return true;
        }

        #endregion
    }
}
