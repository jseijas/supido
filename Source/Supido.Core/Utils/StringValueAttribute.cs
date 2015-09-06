using System;
using System.Linq.Expressions;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Attribute "StringValue".
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class StringValueAttribute : Attribute
    {
        #region - Properties -

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringValueAttribute(string value)
        {
            this.Value = value;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the attribute value for a given property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns></returns>
        public static string Get<T>(Expression<Func<T>> propertyLambda)
        {
            return (string)AttributeUtil.Get(propertyLambda, typeof(StringValueAttribute), "Value");
        }

        /// <summary>
        /// Gets all the attrivute values for a given property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns></returns>
        public static string[] GetAll<T>(Expression<Func<T>> propertyLambda)
        {
            object[] values = AttributeUtil.GetAll(propertyLambda, typeof(StringValueAttribute), "Value");
            if (values == null)
            {
                return null;
            }
            string[] result = new string[values.Length];
            int i = 0;
            foreach (object value in values)
            {
                result[i] = (string)value;
                i++;
            }
            return result;
        }

        #endregion
    }
}
