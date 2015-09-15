using System;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for a recursive call.
    /// </summary>
    public class RecursiveRule : Rule
    {
        #region - Fields -

        /// <summary>
        /// The rule function
        /// </summary>
        private Func<Rule> ruleFunction;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursiveRule"/> class.
        /// </summary>
        /// <param name="ruleFunction">The rule function.</param>
        public RecursiveRule(Func<Rule> ruleFunction)
            : base()
        {
            this.ruleFunction = ruleFunction;
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
            if (this.Sons.Count == 0)
            {
                this.Sons.Add(ruleFunction());
            }
            return this.FirstSon.Match(state);
        }

        #endregion
    }
}
