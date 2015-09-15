using System.Collections.Generic;
using System.Linq;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Class for a node of a variable environment.
    /// </summary>
    public class VariableNode
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public VariableNode Parent { get; set; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<VariableNode> Nodes
        {
            get
            {
                for (var b = this; b != null; b = b.Parent)
                {
                    yield return b;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return this.Find(name); }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        public VariableNode()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public VariableNode(string name, object value)
            : this(null, name, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public VariableNode(VariableNode parent, string name, object value)
        {
            this.Parent = parent;
            this.Name = name;
            this.Value = value;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public VariableNode AddVariable(string name, object value)
        {
            return new VariableNode(this, name, value);
        }

        /// <summary>
        /// Finds the variable or return default.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public VariableNode FindVariableOrDefault(string name)
        {
            return this.Nodes.FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Finds the variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public VariableNode FindVariable(string name)
        {
            VariableNode result = this.FindVariableOrDefault(name);
            if (result == null)
            {
                //result = this.AddVariable(name, null);
                //throw new Exception(string.Format("Variable {0} does not exists in context", name));
            }
            return result;
        }

        /// <summary>
        /// Finds variable by name and return its value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object Find(string name)
        {
            return this.FindVariable(name).Value;
        }

        /// <summary>
        /// Indicates if the variable exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ExistsVariable(string name)
        {
            return FindVariableOrDefault(name) != null;
        }

        #endregion
    }
}
