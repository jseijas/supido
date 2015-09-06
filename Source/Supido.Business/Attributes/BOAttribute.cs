using System;

namespace Supido.Business.Attributes
{
    /// <summary>
    /// Attribute to specify that a class is a Business Object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BOAttribute : Attribute
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the type of the dto.
        /// </summary>
        /// <value>
        /// The type of the dto.
        /// </value>
        public Type DtoType { get; set; }

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
        /// Initializes a new instance of the <see cref="BOAttribute"/> class.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        public BOAttribute(Type dtoType)
        {
            this.DtoType = dtoType;
            this.Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BOAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public BOAttribute(string name)
        {
            this.DtoType = null;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BOAttribute"/> class.
        /// </summary>
        public BOAttribute()
        {
            this.DtoType = null;
            this.Name = string.Empty;
        }

        #endregion
    }
}
