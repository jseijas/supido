
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the Not.
    /// </summary>
    public class NotRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="NotRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public NotRule(Rule rule)
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
            return !this.FirstSon.Match(state.Clone());
        }

        #endregion
    }
}
