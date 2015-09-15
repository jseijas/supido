
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the repeat until.
    /// </summary>
    public class RepeatRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public RepeatRule(Rule rule)
            : base(rule)
        {
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
            while (this.FirstSon.Match(state))
            {
            }
            return true;
        }

        #endregion
    }
}
