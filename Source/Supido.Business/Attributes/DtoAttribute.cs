using System;

namespace Supido.Business.Attributes
{
    /// <summary>
    /// Attribute to specify that a class is a DTO.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DtoAttribute : Attribute
    {
        #region - Properties -

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoAttribute"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public DtoAttribute(Type entityType)
        {
            this.EntityType = entityType;
            this.EntityName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoAttribute"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        public DtoAttribute(string entityName)
        {
            this.EntityType = null;
            this.EntityName = entityName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DtoAttribute"/> class.
        /// </summary>
        public DtoAttribute()
        {
            this.EntityType = null;
            this.EntityName = string.Empty;
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
