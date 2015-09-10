using Supido.Service.Cors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Supido.Service.Utils
{
    /// <summary>
    /// Static class for autodiscover services.
    /// </summary>
    public static class ServiceDiscover
    {
        #region - Static Methods -

        #region - Static Private Methods -

        /// <summary>
        /// Determines whether the specified type is service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static bool IsValidService(Type type)
        {
            if ((type.IsAbstract) || (type.IsInterface) || (type.IsGenericType))
            {
                return false;
            }
            foreach (Type interfacetype in type.GetInterfaces())
            {
                if (Attribute.GetCustomAttribute(interfacetype, typeof(ServiceContractAttribute)) != null)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region - Static Public Methods -

        /// <summary>
        /// Discovers the services.
        /// </summary>
        public static void DiscoverServices(string prefix, AppDomain domain, bool cors)
        {
            IEnumerable<Assembly> possibleAssemblies = domain.GetAssemblies();
            IList<Type> possibleTypes = new List<Type>();
            foreach (Assembly assembly in possibleAssemblies)
            {
                if (!assembly.FullName.StartsWith("System"))
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (IsValidService(type))
                        {
                            string name = string.IsNullOrEmpty(prefix) ? type.Name.ToLower() : prefix + "/" + type.Name.ToLower();
                            if (cors)
                            {
                                RouteTable.Routes.Add(new ServiceRoute(name, new CorsEnabledServiceHostFactory(), type));
                            }
                            else
                            {
                                RouteTable.Routes.Add(new ServiceRoute(name, new WebServiceHostFactory(), type));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Discovers the services.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        public static void DiscoverServices(string prefix, string assemblyName, bool cors)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            IList<Type> possibleTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (IsValidService(type))
                {
                    string name = string.IsNullOrEmpty(prefix) ? type.Name.ToLower() : prefix + "/" + type.Name.ToLower();
                    if (cors)
                    {
                        RouteTable.Routes.Add(new ServiceRoute(name, new CorsEnabledServiceHostFactory(), type));
                    }
                    else
                    {
                        RouteTable.Routes.Add(new ServiceRoute(name, new WebServiceHostFactory(), type));
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
