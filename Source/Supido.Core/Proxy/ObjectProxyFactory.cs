using AutoMapper;
using Supido.Core.Container;
using Supido.Core.Types;
using Supido.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;


namespace Supido.Core.Proxy
{
    /// <summary>
    /// Factory Class for Object Proxy
    /// </summary>
    public static class ObjectProxyFactory
    {
        #region - Static Fields -

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object syncRoot = new Object();

        /// <summary>
        /// The proxies
        /// </summary>
        private static Dictionary<Type, IObjectProxy> proxies = new Dictionary<Type, IObjectProxy>();

        #endregion

        #region - Static Methods -

        /// <summary>
        /// Gets proxy by type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IObjectProxy GetByType(Type type)
        {
            lock (syncRoot)
            {
                if (proxies.ContainsKey(type))
                {
                    return proxies[type];
                }
                else
                {
                    IObjectProxy result = new ObjectProxy(type);
                    proxies.Add(type, result);
                    return result;
                }
            }
        }

        /// <summary>
        /// Gets proxy by generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectProxy Get<T>()
        {
            return GetByType(typeof(T));
        }

        /// <summary>
        /// Gets proxy by instance
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IObjectProxy Get(object instance)
        {
            return GetByType(instance.GetType());
        }

        /// <summary>
        /// Creates an object by generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateObject<T>()
        {
            return (T)Get<T>().CreateObject();
        }

        /// <summary>
        /// Creates a list by generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> CreateList<T>()
        {
            return (IList<T>)Get<T>().CreateList();
        }


        /// <summary>
        /// Fills the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void Fill(object source, object target)
        {
            IObjectProxy sourceProxy = Get(source);
            IObjectProxy targetProxy = Get(target);
            foreach (string propertyName in sourceProxy.PropertyNames)
            {
                targetProxy.SetValue(target, propertyName, sourceProxy.GetValue(source, propertyName));
            }

        }

        /// <summary>
        /// Maps an instance to a target type. Search direct mapper, if not found search inverse mapper, if not found try to map by property names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static T MapTo<T>(object instance)
        {
            return Mapper.Map<T>(instance);
        }

        /// <summary>
        /// Maps to.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static object MapTo(Type targetType, object instance)
        {
            return Mapper.Map(instance, instance.GetType(), targetType);
        }

        /// <summary>
        /// Maps a list of objects to a list of target type.
        /// Search direct mapper, if not found search inverse mapper, if not found try to map by property names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IList<T> MapToList<T>(IEnumerable source)
        {
            if (source == null)
            {
                return null;
            }
            return (IList<T>)Mapper.Map(source, source.GetType(), typeof(IList<T>));
        }

        /// <summary>
        /// Converts an object to a map
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToMap(object instance, bool includeNulls = true)
        {
            return Get(instance).WriteToMap(instance, includeNulls);
        }

        /// <summary>
        /// Converts an object to a map of strings.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="includeNulls">if set to <c>true</c> [include nulls].</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToStringMap(object instance, bool includeNulls = true)
        {
            return Get(instance).WriteToStringMap(instance, includeNulls);
        }

        /// <summary>
        /// Creates an instance of target type and fills from a map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        public static T FromMap<T>(Dictionary<string, object> map)
        {
            IObjectProxy proxy = Get<T>();
            T result = (T)proxy.CreateObject();
            proxy.ReadFromMap(result, map);
            return result;
        }

        /// <summary>
        /// Creates an instance of target type and fills from a string map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        public static T FromStringMap<T>(Dictionary<string, string> map)
        {
            IObjectProxy proxy = Get<T>();
            T result = (T)proxy.CreateObject();
            proxy.ReadFromStringMap(result, map);
            return result;
        }

        /// <summary>
        /// Converts a source list to a list of maps.
        /// </summary>
        /// <param name="srcList">The source list.</param>
        /// <returns></returns>
        public static IList<Dictionary<string, object>> ToListMap(IEnumerable srcList)
        {
            if (srcList == null)
            {
                return null;
            }
            IList<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (object item in srcList)
            {
                result.Add(ToMap(item));
            }
            return result;
        }

        /// <summary>
        /// Converts a source list to a list of string maps.
        /// </summary>
        /// <param name="srcList">The source list.</param>
        /// <returns></returns>
        public static IList<Dictionary<string, string>> ToListStringMap(IEnumerable srcList)
        {
            if (srcList == null)
            {
                return null;
            }
            IList<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (object item in srcList)
            {
                result.Add(ToStringMap(item));
            }
            return result;
        }

        /// <summary>
        /// Converts a list of maps to a list of instances.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        public static IList<T> FromListMap<T>(IList<Dictionary<string, object>> map)
        {
            if (map == null)
            {
                return null;
            }
            IObjectProxy proxy = Get<T>();
            IList<T> result = (IList<T>)proxy.CreateList();
            foreach (Dictionary<string, object> row in map)
            {
                T item = (T)proxy.CreateObject();
                proxy.ReadFromMap(item, row);
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Converts a list of string maps to a list of instances.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        public static IList<T> FromListStringMap<T>(IList<Dictionary<string, string>> map)
        {
            if (map == null)
            {
                return null;
            }
            IObjectProxy proxy = Get<T>();
            IList<T> result = (IList<T>)proxy.CreateList();
            foreach (Dictionary<string, string> row in map)
            {
                T item = (T)proxy.CreateObject();
                proxy.ReadFromStringMap(item, row);
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Configures the mappers.
        /// </summary>
        /// <param name="document">The document.</param>
        public static void ConfigureMappers(XmlDocument document)
        {
            Mapper.Configuration.DisableConstructorMapping();
            if (document != null)
            {
                foreach (XmlNode node in document.SelectNodes("configuration/mappers/mapper"))
                {
                    NodeAttributes attributes = new NodeAttributes(node);
                    Type sourceType = TypesManager.ResolveType(attributes.AsString("source"));
                    Type targetType = TypesManager.ResolveType(attributes.AsString("target"));
                    bool defaultMapping = attributes.AsBool("defaultMapping", true);
                    if ((sourceType != null) && (targetType != null))
                    {
                        // TODO: Add mappers using Automapper.
                        //IMapper mapper = ObjectProxyFactory.NewMapper(sourceType, targetType);
                        //if (defaultMapping)
                        //{
                        //    mapper.DefaultMap();
                        //}
                        //foreach (XmlNode mapNode in node.SelectNodes("mappings/mapping"))
                        //{
                        //    NodeAttributes mapAttributes = new NodeAttributes(mapNode);
                        //    mapper.Map(mapAttributes.AsString("source"), mapAttributes.AsString("target"));
                        //}
                    }
                }
            }
        }

        public static void ConfigureMappers()
        {
            ConfigureMappers((XmlDocument)null);
        }

        /// <summary>
        /// Configures the mappers.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void ConfigureMappers(string fileName)
        {
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document == null)
            {
                document = XmlResources.GetFromEmbeddedResource(fileName);
            }
            ConfigureMappers(document);
        }

        public static void CreateMap(Type a, Type b, IList<string> sources, IList<string> targets)
        {
            IMappingExpression expressionA2B = Mapper.CreateMap(a, b);
            IMappingExpression expressionB2A = Mapper.CreateMap(b, a);
            if (sources != null && targets != null)
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    string sourceName = sources[i];
                    string targetName = targets[i];
                    expressionA2B.ForMember(targetName, opt => opt.MapFrom(sourceName));
                    expressionB2A.ForMember(sourceName, opt => opt.MapFrom(targetName));
                }
            }
        }

        public static void CreateMap(Type a, Type b)
        {
            CreateMap(a, b, null, null);
        }
        
        #endregion
    }
}
