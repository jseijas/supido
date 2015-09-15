
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the optional rules.
    /// </summary>
    public class OptRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="OptRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public OptRule(Rule rule)
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
            this.FirstSon.Match(state);
            return true;
        }

        #endregion
    }
}
