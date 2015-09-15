using System.Collections.Generic;

namespace Supido.Templates
{
    /// <summary>
    /// Class for a link to another container
    /// </summary>
    public class TemplateLink
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TemplateContainer Parent { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the list values.
        /// </summary>
        /// <value>
        /// The list values.
        /// </value>
        public IList<string> ListValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is list; otherwise, <c>false</c>.
        /// </value>
        public bool IsList
        {
            get { return this.ListValues != null; }
            set
            {
                if (value)
                {
                    if (this.ListValues == null)
                    {
                        this.ListValues = new List<string>();
                    }
                }
                else
                {
                    if (this.ListValues != null)
                    {
                        this.ListValues = null;
                    }
                }
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateLink"/> class.
        /// </summary>
        public TemplateLink()
        {
            this.Parent = null;
            this.Link = null;
            this.ListValues = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateLink"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="link">The link.</param>
        public TemplateLink(TemplateContainer parent, string link)
        {
            this.Parent = parent;
            this.Link = link;
            this.ListValues = null;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the list value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddListValue(string value)
        {
            if (this.ListValues == null)
            {
                this.ListValues = new List<string>();
            }
            this.ListValues.Add(value);
        }

        #endregion
    }
}
