
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the while.
    /// </summary>
    public class WhileRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="WhileRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public WhileRule(Rule rule)
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
            if (!this.FirstSon.Match(state))
            {
                return false;
            }
            while (this.FirstSon.Match(state))
            {
            }
            return true;
        }

        #endregion
    }
}
