using System;
using System.Collections.Generic;
using System.Reflection;

namespace Supido.Core.Proxy
{
    /// <summary>
    /// Interface for the proxy of an object, that allows to know the property type by name, gets and sets properties
    /// by name, and create new instances of the object or collection of objects.
    /// </summary>
    public interface IObjectProxy : IObjectAccessor, IObjectCreator
    {
        #region - Properties -

        /// <summary>
        /// Gets the type of the proxy.
        /// </summary>
        /// <value>
        /// The type of the proxy.
        /// </value>
        Type ProxyType { get; }

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <value>
        /// The property names.
        /// </value>
        string[] PropertyNames { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IList<PropertyInfo> Properties { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        Type GetPropertyType(string name);

        /// <summary>
        /// Determines whether [is null properties] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        bool IsNullProperties(object instance);

        /// <summary>
        /// Determines whether [is default properties] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        bool IsDefaultProperties(object instance);

        /// <summary>
        /// Writes to map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        Dictionary<string, object> WriteToMap(object instance, bool includeNulls = true);

        /// <summary>
        /// Writes to string map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        Dictionary<string, string> WriteToStringMap(object instance, bool includeNulls = true);

        /// <summary>
        /// Reads from map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="map">The map.</param>
        void ReadFromMap(object instance, Dictionary<string, object> map);

        /// <summary>
        /// Reads from string map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="map">The map.</param>
        void ReadFromStringMap(object instance, Dictionary<string, string> map);

        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        object CloneObject(object instance);

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        PropertyInfo GetProperty(string name);

        #endregion
    }
}
