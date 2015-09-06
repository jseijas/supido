using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Supido.Core.Types
{
    /// <summary>
    /// Class for a type resolver with cache.
    /// </summary>
    public class TypeResolver
    {
        #region - Fields -

        /// <summary>
        /// The type cache.
        /// </summary>
        private Dictionary<string, Type> cache = new Dictionary<string, Type>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Resolves a generic type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        /// <exception cref="System.TypeLoadException"></exception>
        protected Type ResolveGeneric(string typeName)
        {
            if (typeName.StartsWith(BaseAssemblyInfo.NullableTypeStarter))
            {
                return null;
            }
            GenericArguments arguments = new GenericArguments(typeName);
            Type result = null;
            try
            {
                if (arguments.ContainsArguments)
                {
                    result = this.Resolve(arguments.TypeName);
                    if (!arguments.IsDefinition)
                    {
                        string[] unresolvedArguments = arguments.GetArguments();
                        Type[] genericArguments = new Type[unresolvedArguments.Length];
                        for (int i = 0; i < unresolvedArguments.Length; i++)
                        {
                            genericArguments[i] = this.Resolve(unresolvedArguments[i]);
                        }
                        result = result.MakeGenericType(genericArguments);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is TypeLoadException)
                {
                    throw;
                }
                throw new TypeLoadException(string.Format("Could not resolve type '{0}'.", typeName), ex);
            }
            return result;
        }

        /// <summary>
        /// Resolves a non generic type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        /// <exception cref="System.TypeLoadException">
        /// </exception>
        protected Type ResolveNonGeneric(string typeName)
        {
            BaseAssemblyInfo typeInfo = new BaseAssemblyInfo(true);
            typeInfo.OriginalName = typeName;
            Type result = null;
            try
            {
                result = typeInfo.IsAssemblyReady ? LoadFromAssembly(typeInfo) : LoadFromAllAssemblies(typeInfo);
                if (result == null)
                {
                    string[] tokens = typeInfo.OriginalName.Split('.');
                    //typeInfo.Name = tokens[tokens.Length - 1];
                    string s = tokens[0];
                    for (int i = 1; i < tokens.Length - 1; i++)
                    {
                        s = s + "." + tokens[i];
                    }
                    typeInfo.AssemblyName = s;
                    result = LoadFromAssembly(typeInfo);
                }
            }
            catch
            {
                //throw new TypeLoadException(string.Format("Could not resolve type '{0}'.", typeName), ex);
                return null;
            }
            return result;
        }

        /// <summary>
        /// Resolves the specified type by its name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        /// <exception cref="System.TypeLoadException">
        /// Could not load type from null type name.
        /// or
        /// Could not load type from empty type name.
        /// </exception>
        public Type Resolve(string typeName)
        {
            if (typeName == null)
            {
                throw new TypeLoadException("Could not load type from null type name.");
            }
            typeName = typeName.Trim();
            if (typeName == string.Empty)
            {
                throw new TypeLoadException("Could not load type from empty type name.");
            }
            typeName = typeName.Replace(" ", string.Empty);
            if (this.cache.ContainsKey(typeName))
            {
                return this.cache[typeName];
            }
            Type result = this.ResolveGeneric(typeName);
            if (result == null)
            {
                result = this.ResolveNonGeneric(typeName);
                if (result == null)
                {
                    return null;
                }
            }
            this.cache.Add(typeName, result);
            return result;
        }

        /// <summary>
        /// Loads a type from assembly.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <returns></returns>
        private Type LoadFromAssembly(BaseAssemblyInfo typeInfo)
        {
            Assembly assembly = Assembly.Load(typeInfo.AssemblyName);
            return assembly == null ? null : assembly.GetType(typeInfo.Name, true, true);
        }

        /// <summary>
        /// Loads a type from all assemblies of the current domain.
        /// </summary>
        /// <param name="typeInfo">The type info.</param>
        /// <returns></returns>
        private Type LoadFromAllAssemblies(BaseAssemblyInfo typeInfo)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(typeInfo.Name, false, true);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a type to the cache.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public void AddType(string name, Type type)
        {
            if (!this.cache.ContainsKey(name))
            {
                this.cache.Add(name, type);
            }
        }

        /// <summary>
        /// Removes the type from the cache.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveType(string name)
        {
            this.cache.Remove(name);
        }

        #endregion
    }
}
