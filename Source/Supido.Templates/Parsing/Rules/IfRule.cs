
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the conditional If.
    /// </summary>
    public class IfRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="IfRule"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public IfRule(params Rule[] rules)
            : base(rules)
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
            ParserState oldState = state.Clone();
            foreach (Rule rule in this.Sons)
            {
                if (rule.Match(state))
                {
                    return true;
                }
                state.Assign(oldState);
            }
            return false;
        }

        #endregion
    }
}
