
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for matching at token.
    /// </summary>
    public class AtRule : Rule
    {
        #region - Methods -

        /// <summary>
        /// Initializes a new instance of the <see cref="AtRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public AtRule(Rule rule)
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
            return this.FirstSon.Match(state.Clone());
        }

        #endregion
    }
}
