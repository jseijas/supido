using System.Collections.Generic;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Class for a token rule.
    /// </summary>
    public class NodeRule : Rule
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRule"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public NodeRule(Rule rule)
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
            Token node = new Token(this.Name, state);
            IList<Token> oldNodes = state.Nodes;
            state.Nodes = new List<Token>();
            if (this.FirstSon.Match(state))
            {
                node.End = state.CurrentPosition;
                node.Tokens = state.Nodes;
                oldNodes.Add(node);
                state.Nodes = oldNodes;
                return true;
            }
            else
            {
                state.Nodes = oldNodes;
                return false;
            }
        }

        #endregion
    }
}
