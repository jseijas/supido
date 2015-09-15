using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Supido.Templates.Parsing
{
    /// <summary>
    /// Class for a parser token
    /// </summary>
    public class Token
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
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the begin.
        /// </summary>
        /// <value>
        /// The begin.
        /// </value>
        public int Begin { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public int End { get; set; }

        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        /// <value>
        /// The tokens.
        /// </value>
        public IList<Token> Tokens { get; set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length
        {
            get { return this.End > this.Begin ? this.End - this.Begin : 0; }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return this.Input.Substring(this.Begin, this.Length); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeaf
        {
            get { return this.Tokens.Count == 0; }
        }

        /// <summary>
        /// Gets to XML.
        /// </summary>
        /// <value>
        /// To XML.
        /// </value>
        public XElement ToXml
        {
            get
            {
                return this.IsLeaf ? new XElement(this.Name, this.Text) : new XElement(this.Name, from node in this.Tokens select node.ToXml);
            }
        }

        /// <summary>
        /// Gets the <see cref="Token"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="Token"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Token this[string name]
        {
            get { return this.GetToken(name); }
        }

        /// <summary>
        /// Gets the <see cref="Token"/> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="Token"/>.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public Token this[int i]
        {
            get { return this.Tokens[i]; }
        }

        /// <summary>
        /// Gets the node count.
        /// </summary>
        /// <value>
        /// The node count.
        /// </value>
        public int NodeCount
        {
            get { return this.Tokens.Count; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="state">The state.</param>
        /// <exception cref="System.ArgumentNullException">Name of node cannot be null</exception>
        public Token(string name, ParserState state)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Name of node cannot be null");
            }
            this.Name = name;
            this.Input = state.Input;
            this.Begin = state.CurrentPosition;
            this.Tokens = new List<Token>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public Token(string name, IEnumerable<Token> content)
        {
            this.Name = name;
            this.Tokens = content.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public Token(string name, params Token[] content)
            : this(name, content as IEnumerable<Token>)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public Token(string name, string content)
        {
            this.Name = name;
            this.Input = content;
            this.Begin = 0;
            this.End = content.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public Token(XElement element)
        {
            this.Name = element.Name.LocalName;
            if (element.HasElements)
            {
                this.Tokens = (from elem in element.Elements() select new Token(elem)).ToList();
            }
            else
            {
                this.Input = element.Value;
                this.Begin = 0;
                this.End = this.Input.Length;
            }
        }

        #endregion

        #region - Methods -

        public IEnumerable<Token> GetTokens(string name)
        {
            return this.Tokens.Where(n => n.Name == name);
        }

        public Token GetToken(string name)
        {
            return this.Tokens.Where(n => n.Name == name).FirstOrDefault();
        }

        #endregion
    }
}
