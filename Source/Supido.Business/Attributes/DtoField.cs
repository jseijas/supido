using System;

namespace Supido.Business.Attributes
{
    /// <summary>
    /// Attribute for specifying a mapping of a field of a dto.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DtoFieldAttribute : Attribute
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoFieldAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DtoFieldAttribute(string name)
        {
            this.Name = name;
        }

        #endregion
    }
}
