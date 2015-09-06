using Supido.Business.Filter;
using Supido.Core.Container;
using System;
using System.Collections.Generic;

namespace Supido.Business.BO
{
    /// <summary>
    /// Class for a manager of Business Objects.
    /// </summary>
    public class BOManager : IBOManager
    {
        #region - Fields -

        private Dictionary<Type, Type> dtoContextMap = new Dictionary<Type, Type>();

        private Dictionary<Type, Type> dtoFilterMap = new Dictionary<Type, Type>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a filter type related to a DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="filterType">Type of the filter.</param>
        public void AddFilterType(Type dtoType, Type filterType)
        {
            if (this.dtoFilterMap.ContainsKey(dtoType))
            {
                this.dtoFilterMap[dtoType] = filterType;
            }
            else
            {
                this.dtoFilterMap.Add(dtoType, filterType);
            }
        }

        /// <summary>
        /// Adds a filter type related to a DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="filterType">Type of the filter.</param>
        public void AddFilterType<TDto>(Type filterType)
        {
            this.AddFilterType(typeof(TDto), filterType);
        }

        /// <summary>
        /// Adds a BO type related to a DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="contextBOType">Type of the context bo.</param>
        public void AddBOType(Type dtoType, Type contextBOType)
        {
            if (this.dtoContextMap.ContainsKey(dtoType))
            {
                this.dtoContextMap[dtoType] = contextBOType;
            }
            else
            {
                this.dtoContextMap.Add(dtoType, contextBOType);
            }
        }

        /// <summary>
        /// Adds a BO type related to a DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="contextBOType">Type of the context bo.</param>
        public void AddBOType<TDto>(Type contextBOType)
        {
            this.AddBOType(typeof(TDto), contextBOType);
        }

        /// <summary>
        /// Gets the type of the BO from the DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        public Type GetBOType<TDto>()
        {
            if (this.dtoContextMap.ContainsKey(typeof(TDto)))
            {
                return this.dtoContextMap[typeof(TDto)];
            }
            Type result = typeof(ContextBO<TDto>);
            this.AddBOType(typeof(TDto), result);
            return result;
        }

        /// <summary>
        /// Gets the type of the filter from the DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        public Type GetFilterType<TDto>()
        {
            if (this.dtoFilterMap.ContainsKey(typeof(TDto)))
            {
                return this.dtoFilterMap[typeof(TDto)];
            }
            Type entityType = IoC.Get<ISecurityManager>().MetamodelManager.GetEntityByDto<TDto>().EntityType;
            Type filterType = typeof(UnsecureContextBOFilter<>).MakeGenericType(entityType);
            this.AddFilterType<TDto>(filterType);
            return filterType;
        }

        #endregion
    }
}
