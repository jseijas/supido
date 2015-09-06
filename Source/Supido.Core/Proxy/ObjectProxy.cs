using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Supido.Core.Proxy
{
    /// <summary>
    /// Class for an ObjectProxy, the way to do faster the reflection to instances.
    /// </summary>
    public class ObjectProxy : IObjectProxy
    {
        #region - Fields -

        /// <summary>
        /// The properties
        /// </summary>
        private List<PropertyInfo> properties = new List<PropertyInfo>();

        /// <summary>
        /// The property map
        /// </summary>
        private Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// The map of lower names
        /// </summary>
        private Dictionary<string, string> lowerNames = new Dictionary<string, string>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the type of the proxy.
        /// </summary>
        /// <value>
        /// The type of the proxy.
        /// </value>
        public Type ProxyType { get; private set; }

        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <value>
        /// The type of the collection.
        /// </value>
        public Type CollectionType { get; private set; }

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <value>
        /// The property names.
        /// </value>
        public string[] PropertyNames
        {
            get { return this.propertyMap.Keys.ToArray(); }
        }

        public IList<PropertyInfo> Properties
        {
            get { return this.properties; }
        }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="proxyType">Type of the proxy.</param>
        /// <param name="collectionType">Type of the collection.</param>
        public ObjectProxy(Type proxyType, Type collectionType)
        {
            this.ProxyType = proxyType;
            this.CollectionType = collectionType;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProxy"/> class.
        /// </summary>
        /// <param name="proxyType">Type of the proxy.</param>
        public ObjectProxy(Type proxyType)
            : this(proxyType, typeof(List<>).MakeGenericType(proxyType))
        {
        }

        #endregion

        #region - Methods -

        #region - Private Methods -

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="info">The information.</param>
        private void AddProperty(PropertyInfo info)
        {
            this.properties.Add(info);
            this.propertyMap.Add(info.Name, info);
            this.lowerNames.Add(info.Name.ToLower(), info.Name);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            foreach (PropertyInfo info in this.ProxyType.GetProperties())
            {
                this.AddProperty(info);
            }
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            string lowName = name.ToLower();
            if (this.lowerNames.ContainsKey(lowName))
            {
                return this.propertyMap[this.lowerNames[lowName]];
            }
            return null;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        private object GetValue(object instance, PropertyInfo pi)
        {
            if (pi != null)
            {
                if (pi.CanRead)
                {
                    return pi.GetValue(instance);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        /// <summary>
        /// Changes the type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        private object ChangeType(object value, Type targetType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                if (value.GetType() == underlyingType)
                {
                    return value;
                }
                return Convert.ChangeType(value, underlyingType);
            }
            else
            {
                if (value.GetType() == targetType)
                {
                    return value;
                }
                return Convert.ChangeType(value, targetType);
            }
        }

        #endregion

        #region - Methods from IObjectAccessor -

        /// <summary>
        /// Gets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        public object GetValue(object instance, string name)
        {
            return this.GetValue(instance, this.GetProperty(name));
        }

        /// <summary>
        /// Sets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object instance, string name, object value)
        {
            PropertyInfo pi = this.GetProperty(name);
            if ((pi == null) || (!pi.CanWrite))
            {
                return;
            }
            if (value == null)
            {
                pi.SetValue(instance, null);
            }
            else
            {
                pi.SetValue(instance, this.ChangeType(value, pi.PropertyType));
            }
        }

        #endregion

        #region - Methods from IObjectCreator -

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns></returns>
        public object CreateObject()
        {
            return Activator.CreateInstance(this.ProxyType);
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <returns></returns>
        public IList CreateList()
        {
            return (IList)Activator.CreateInstance(this.CollectionType);
        }

        #endregion

        #region - Methods from IObjectProxy -

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        public Type GetPropertyType(string name)
        {
            PropertyInfo pi = this.GetProperty(name);
            if (pi == null)
            {
                return null;
            }
            return pi.PropertyType;
        }

        /// <summary>
        /// Determines whether [is null properties] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public bool IsNullProperties(object instance)
        {
            foreach (PropertyInfo pi in this.properties)
            {
                if (this.GetValue(instance, pi) != null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether [is default properties] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public bool IsDefaultProperties(object instance)
        {
            foreach (PropertyInfo pi in this.properties)
            {
                object value = this.GetValue(instance, pi);
                object defaultValue = this.GetDefaultValue(pi.PropertyType);
                if (defaultValue == null)
                {
                    if (value != null)
                    {
                        return false;
                    }
                }
                else if (!defaultValue.Equals(value))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Writes to map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        public Dictionary<string, object> WriteToMap(object instance, bool includeNulls = true)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (PropertyInfo pi in this.properties)
            {
                object value = this.GetValue(instance, pi);
                if (value == null)
                {
                    if (includeNulls)
                    {
                        result.Add(pi.Name, value);
                    }
                }
                else
                {
                    result.Add(pi.Name, value);
                }
            }
            return result;
        }

        /// <summary>
        /// Writes to string map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        public Dictionary<string, string> WriteToStringMap(object instance, bool includeNulls = true)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (PropertyInfo pi in this.properties)
            {
                object value = this.GetValue(instance, pi);
                if (value == null)
                {
                    if (includeNulls)
                    {
                        result.Add(pi.Name, null);
                    }
                }
                else
                {
                    result.Add(pi.Name, value.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Reads from map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="map">The map.</param>
        public void ReadFromMap(object instance, Dictionary<string, object> map)
        {
            foreach (KeyValuePair<string, object> entry in map)
            {
                this.SetValue(instance, entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Reads from string map.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="map">The map.</param>
        public void ReadFromStringMap(object instance, Dictionary<string, string> map)
        {
            foreach (KeyValuePair<string, string> entry in map)
            {
                this.SetValue(instance, entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object CloneObject(object instance)
        {
            object result = this.CreateObject();
            foreach (PropertyInfo pi in this.properties)
            {
                this.SetValue(result, pi.Name, this.GetValue(instance, pi));
            }
            return result;
        }

        #endregion

        #endregion
    }
}
