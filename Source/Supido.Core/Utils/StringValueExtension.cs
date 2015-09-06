using System;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Extension for the StringValue attribute.
    /// </summary>
    public static class StringValueExtension
    {
        /// <summary>
        /// From a Enum value gives the String Value attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringValue(this Enum value)
        {
            StringValueAttribute[] attributes = (StringValueAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
            return ((attributes != null) && (attributes.Length > 0)) ? attributes[0].Value : value.ToString();
        }

        /// <summary>
        /// Given a string search the enum value associated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                StringValueAttribute[] attributes = (StringValueAttribute[])item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
                if ((attributes != null) && (attributes.Length > 0) && (attributes[0].Value.Equals(str)))
                {
                    return item;
                }
            }
            return (T)Enum.Parse(typeof(T), str, true);
        }
    }
}
