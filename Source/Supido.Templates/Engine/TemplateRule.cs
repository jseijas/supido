using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Supido.Templates.Engine
{
    /// <summary>
    /// Class for a rule of a template
    /// </summary>
    public class TemplateRule
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent engine.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TemplateEngine Parent { get; private set; }

        /// <summary>
        /// Gets or sets the iterator.
        /// </summary>
        /// <value>
        /// The iterator.
        /// </value>
        public string Iterator { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public string Condition { get; set; }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        public IList<TemplateAction> Actions { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateRule"/> class.
        /// </summary>
        public TemplateRule(TemplateEngine parent)
        {
            this.Parent = parent;
            this.Actions = new List<TemplateAction>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateRule"/> class.
        /// </summary>
        /// <param name="iterator">The iterator.</param>
        /// <param name="condition">The condition.</param>
        public TemplateRule(TemplateEngine parent, string iterator, string condition)
            : this(parent)
        {
            this.Iterator = iterator;
            this.Condition = condition;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Loads the actions.
        /// </summary>
        /// <param name="node">The node.</param>
        public void LoadActions(XmlNode node, string path)
        {
            foreach (XmlNode actionNode in node.SelectNodes("actions/action"))
            {
                NodeAttributes attributes = new NodeAttributes(actionNode);
                TemplateAction action = new TemplateAction(this, attributes.AsString("type"), path + "\\" + attributes.AsString("source"), attributes.AsString("target"));
                this.Actions.Add(action);
            }
        }

        /// <summary>
        /// Executes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Execute(TemplateContainer container)
        {
            foreach (TemplateAction action in this.Actions)
            {
                action.Execute(container);
            }
        }

        /// <summary>
        /// Iterates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentException">The iterator  + this.Iterator +  is not a valid container</exception>
        public void Iterate(TemplateContainer container)
        {
            object pathItem = string.IsNullOrEmpty(this.Iterator) ? container : container.GetByPath(this.Iterator);
            if ((pathItem != null) && (pathItem is TemplateContainer))
            {
                TemplateContainer root = (TemplateContainer)pathItem;
                if (root.IsList)
                {
                    string maxIteratorIndex = (root.ArrayValues.Count - 1).ToString();
                    for (int i = 0; i < root.ArrayValues.Count; i++)
                    {
                        TemplateContainer son = root.ArrayValues[i];
                        son.AddAttribute("IteratorIndex", i.ToString());
                        son.AddAttribute("MaxIteratorIndex", maxIteratorIndex);
                        son.AddAttribute("IsIteratorFirst", i == 0);
                        son.AddAttribute("IsIteratorLast", i == root.ArrayValues.Count - 1);
                        this.Execute(son);
                    }
                }
                else
                {
                    this.Execute(root);
                }
            }
            else
            {
                throw new ArgumentException("The iterator " + this.Iterator + " is not a valid container");
            }
        }

        #endregion
    }
}
