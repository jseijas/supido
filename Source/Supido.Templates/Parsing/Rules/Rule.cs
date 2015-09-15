using Supido.Templates.Parsing.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Rule of a grammar.
    /// </summary>
    public abstract class Rule
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sons.
        /// </summary>
        /// <value>
        /// The sons.
        /// </value>
        public IList<Rule> Sons { get; set; }

        /// <summary>
        /// Gets the first son.
        /// </summary>
        /// <value>
        /// The first son.
        /// </value>
        public Rule FirstSon 
        {
            get { return this.Sons[0]; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <exception cref="System.Exception">The son rules cannot be null.</exception>
        public Rule(params Rule[] rules)
        {
            if (rules.Any(r => r == null))
            {
                throw new Exception("The son rules cannot be null.");
            }
            this.Sons = new List<Rule>(rules);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Indicates if the current parser state matches the rule.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public abstract bool Match(ParserState state);

        /// <summary>
        /// Indicates if the given string matches the rule.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool Match(string input)
        {
            return this.Match(new ParserState(input));
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public IList<Token> Parse(string input)
        {
            var state = new ParserState(input);
            if (!Match(state))
            {
                throw new Exception(String.Format("Rule {0} failed to match", Name));
            }
            return state.Nodes;
        }


        /// <summary>
        /// Sets the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Rule SetName(string name)
        {
            this.Name = name;
            return this;
        }

        #endregion

        #region - Operations -

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Rule operator +(Rule r1, Rule r2)
        {
            return BaseGrammar.Sequence(r1, r2);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Rule operator |(Rule r1, Rule r2)
        {
            return BaseGrammar.Condition(r1, r2);
        }

        #endregion
    }
}
