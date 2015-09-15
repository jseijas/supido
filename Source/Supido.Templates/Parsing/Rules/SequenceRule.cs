
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for a sequence execution.
    /// </summary>
    public class SequenceRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceRule"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public SequenceRule(params Rule[] rules)
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
                if (!rule.Match(state))
                {
                    state.Assign(oldState);
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
