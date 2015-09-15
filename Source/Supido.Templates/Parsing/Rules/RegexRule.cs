using System.Text.RegularExpressions;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for a regular expression
    /// </summary>
    public class RegexRule : Rule
    {
        #region - Fields -

        /// <summary>
        /// The regular expression
        /// </summary>
        private Regex regex;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexRule"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        public RegexRule(Regex regex)
        {
            this.regex = regex;
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
            Match match = this.regex.Match(state.Input, state.CurrentPosition);
            if (match == null || match.Index != state.CurrentPosition)
            {
                return false;
            }
            state.CurrentPosition = state.CurrentPosition + match.Length;
            return true;
        }

        #endregion
    }
}
