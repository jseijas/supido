using System.Collections.Generic;

namespace Supido.Structure.Meta
{
    /// <summary>
    /// Class for the relationship between two tables of the model.
    /// </summary>
    public class MetaRelation
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>
        /// The name of the parent.
        /// </value>
        public string ParentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the son relation.
        /// </summary>
        /// <value>
        /// The name of the son relation.
        /// </value>
        public string SonRelationName { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public MetaTable Parent { get; set; }

        /// <summary>
        /// Gets or sets the son.
        /// </summary>
        /// <value>
        /// The son.
        /// </value>
        public MetaTable Son { get; set; }

        /// <summary>
        /// Gets or sets the parent columns.
        /// </summary>
        /// <value>
        /// The parent columns.
        /// </value>
        public IList<MetaColumn> ParentColumns { get; set; }

        /// <summary>
        /// Gets or sets the son columns.
        /// </summary>
        /// <value>
        /// The son columns.
        /// </value>
        public IList<MetaColumn> SonColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is key in son.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is key in son; otherwise, <c>false</c>.
        /// </value>
        public bool IsKeyInSon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable in son.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable in son; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullableInSon { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaRelation"/> class.
        /// </summary>
        public MetaRelation()
        {
            this.Parent = null;
            this.Son = null;
            this.ParentColumns = new List<MetaColumn>();
            this.SonColumns = new List<MetaColumn>();
            this.IsKeyInSon = false;
        }

        #endregion

    }
}
