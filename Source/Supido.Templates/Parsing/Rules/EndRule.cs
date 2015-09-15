
namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for the end clause
    /// </summary>
    public class EndRule : Rule
    {
        #region - Methods -

        /// <summary>
        /// Indicates if the current parser state matches the rule.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public override bool Match(ParserState state)
        {
            return state.CurrentPosition == state.Input.Length;
        }

        #endregion
    }
}
