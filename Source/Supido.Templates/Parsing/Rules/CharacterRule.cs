using System;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule for matching a character.
    /// </summary>
    public class CharacterRule : Rule
    {
        #region - Fields -

        /// <summary>
        /// The predicate
        /// </summary>
        private Predicate<char> predicate;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterRule"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public CharacterRule(Predicate<char> predicate)
        {
            this.predicate = predicate;
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
            if (state.CurrentPosition >= state.Input.Length)
            {
                return false;
            }
            if (!predicate(state.Input[state.CurrentPosition]))
            {
                return false;
            }
            state.CurrentPosition++;
            return true;
        }

        #endregion
    }
}
