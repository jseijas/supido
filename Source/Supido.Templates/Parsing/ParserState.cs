using System.Collections.Generic;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Class for a parser state.
    /// </summary>
    public class ParserState
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public int CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IList<Token> Nodes { get; set; }

        /// <summary>
        /// Gets the current input.
        /// </summary>
        /// <value>
        /// The current input.
        /// </value>
        public string CurrentInput
        {
            get { return this.Input.Substring(this.CurrentPosition); }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserState"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public ParserState(string input)
        {
            this.Input = input;
            this.CurrentPosition = 0;
            this.Nodes = new List<Token>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public ParserState Clone()
        {
            ParserState result = new ParserState(this.Input);
            result.CurrentPosition = this.CurrentPosition;
            foreach (Token node in this.Nodes)
            {
                result.Nodes.Add(node);
            }
            return result;
        }

        /// <summary>
        /// Assigns the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void Assign(ParserState state)
        {
            this.Input = state.Input;
            this.CurrentPosition = state.CurrentPosition;
            this.Nodes.Clear();
            foreach (Token node in state.Nodes)
            {
                this.Nodes.Add(node);
            }
        }

        #endregion
    }
}
