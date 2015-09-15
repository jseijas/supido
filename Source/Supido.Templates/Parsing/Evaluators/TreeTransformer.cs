using System.Linq;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Class for a tree token transformer.
    /// </summary>
    public class TreeTransformer
    {
        #region - Methods -

        /// <summary>
        /// Does the internal transformation of the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected virtual Token InternalTransform(Token token)
        {
            return token;
        }

        /// <summary>
        /// Transforms all tokens.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        protected Token TransformAllTokens(Token token)
        {
            token.Tokens = token.Tokens.Select(this.TransformAllTokens).ToList();
            return this.InternalTransform(token);
        }

        /// <summary>
        /// Breaks the token into two groups of tokens, with pivot by token name.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected Token LeftGroup(Token token, string name)
        {
            Token leftChild = this.InternalTransform(new Token(token.Name, token.Tokens.Take(token.NodeCount - 1)));
            return new Token(name, leftChild, token.Tokens.Last());
        }

        #endregion
    }
}
