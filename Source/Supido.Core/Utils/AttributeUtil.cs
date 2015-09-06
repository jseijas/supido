using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Util for working with attributes.
    /// </summary>
    public static class AttributeUtil
    {
        #region - Methods -

        /// <summary>
        /// From a lambda expresion of a property, return the property info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The lambda expression of the property.</param>
        /// <returns>Property info of the property</returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyLambda)
        {
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' doesn't refers to a property.", propertyLambda.ToString()));
            }
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));
            }
            return propInfo;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public static object[] GetCustomAttributes<T>(Expression<Func<T>> propertyLambda, Type attributeType = null)
        {
            PropertyInfo info = GetPropertyInfo(propertyLambda);
            return attributeType == null ? info.GetCustomAttributes(true) : info.GetCustomAttributes(attributeType, true);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public static object GetCustomAttribute<T>(Expression<Func<T>> propertyLambda, Type attributeType = null)
        {
            PropertyInfo info = GetPropertyInfo(propertyLambda);
            object[] items = GetCustomAttributes<T>(propertyLambda, attributeType);
            if ((items == null) || (items.Length == 0))
            {
                return null;
            }
            return items[0];
        }

        /// <summary>
        /// From the lambda expression of a property, return the value of a custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the custom attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object Get<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
        {
            object attribute = GetCustomAttribute(propertyLambda, attributeType);
            if (attribute == null)
            {
                return null;
            }
            PropertyInfo info = attributeType.GetProperty(propertyName);
            return info.GetValue(attribute);
        }

        /// <summary>
        /// From the lambda expression of a property, return the values of all custom attribute applicable to the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object[] GetAll<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
        {
            object[] attributes = GetCustomAttributes(propertyLambda, attributeType);
            if (attributes == null)
            {
                return null;
            }
            object[] result = new object[attributes.Length];
            int i = 0;
            if (attributeType != null)
            {
                PropertyInfo info = attributeType.GetProperty(propertyName);
                foreach (object attribute in attributes)
                {
                    result[i] = info.GetValue(attribute);
                    i++;
                }
            }
            else
            {
                foreach (object attribute in attributes)
                {
                    PropertyInfo info = attribute.GetType().GetProperty(propertyName);
                    result[i] = info.GetValue(attribute);
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets Attributes from a property lambda, as string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetAsString<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
        {
            object value = Get<T>(propertyLambda, attributeType, propertyName);
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        /// <summary>
        /// Gets all values of attribute from a property lambda, as string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string[] GetAllAsString<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
        {
            object[] values = GetAll<T>(propertyLambda, attributeType, propertyName);
            if (values == null)
            {
                return null;
            }
            string[] result = new string[values.Length];
            int i = 0;
            foreach (object item in values)
            {
                if (item == null)
                {
                    result[i] = null;
                }
                else
                {
                    result[i] = item.ToString();
                }

                i++;
            }
            return result;
        }

        public static T GetAttributeFrom<T>(Type type) where T:Attribute
        {
            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            if (attributes == null || attributes.Length == 0)
            {
                return null;
            }
            return (T)attributes[0];
        }
 
        #endregion
    }
}
