using System;

namespace Supido.Business.Attributes
{
    /// <summary>
    /// Attribute to specify that a class is a filter of a business object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FilterAttribute : Attribute
    {
        #region - Properties -

        public Type DtoType { get; set; }

        public string Name { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAttribute"/> class.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        public FilterAttribute(Type dtoType)
        {
            this.DtoType = dtoType;
            this.Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FilterAttribute(string name)
        {
            this.DtoType = null;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAttribute"/> class.
        /// </summary>
        public FilterAttribute()
        {
            this.DtoType = null;
            this.Name = string.Empty;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the attribute from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static DtoAttribute GetAttributeFrom(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(DtoAttribute), true);
            if (attributes == null || attributes.Length == 0)
            {
                return null;
            }
            return attributes[0] as DtoAttribute;
        }

        /// <summary>
        /// Gets the attribute from a type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DtoAttribute GetAttributeFrom<T>()
        {
            return GetAttributeFrom(typeof(T));
        }

        #endregion
    }
}
