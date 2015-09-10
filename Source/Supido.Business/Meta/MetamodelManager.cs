using Supido.Business.Attributes;
using Supido.Business.Audit;
using Supido.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Supido.Business.Meta
{
    /// <summary>
    /// Class for a Metamodel Manager.
    /// </summary>
    public class MetamodelManager : IMetamodelManager
    {
        #region - Fields -

        /// <summary>
        /// Maps helpers by entity type.
        /// </summary>
        private Dictionary<Type, IMetamodelEntity> entityMap = new Dictionary<Type, IMetamodelEntity>();

        /// <summary>
        /// Maps helpers by entity name.
        /// </summary>
        private Dictionary<string, IMetamodelEntity> entityNameMap = new Dictionary<string, IMetamodelEntity>();

        /// <summary>
        /// Maps helpers by dto type.
        /// </summary>
        private Dictionary<Type, IMetamodelEntity> dtoMap = new Dictionary<Type, IMetamodelEntity>();

        /// <summary>
        /// Maps helpers by dto name.
        /// </summary>
        private Dictionary<string, IMetamodelEntity> dtoNameMap = new Dictionary<string, IMetamodelEntity>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the entity metamodel by entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IMetamodelEntity GetEntity<TEntity>()
        {
            return this.GetEntity(typeof(TEntity));
        }

        /// <summary>
        /// Gets the entity metamodel by entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IMetamodelEntity GetEntity(Type type)
        {
            return this.entityMap.ContainsKey(type) ? this.entityMap[type] : null;
        }

        /// <summary>
        /// Gets the entity metamodel by dto type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        public IMetamodelEntity GetEntityByDto<TDto>()
        {
            return this.GetEntityByDto(typeof(TDto));
        }

        /// <summary>
        /// Gets the entity metamodel by dto type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IMetamodelEntity GetEntityByDto(Type type)
        {
            return this.dtoMap.ContainsKey(type) ? this.dtoMap[type] : null;
        }

        /// <summary>
        /// Gets the entity metamodel by entity name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IMetamodelEntity GetEntityByName(string name)
        {
            name = name.ToLower();
            return this.entityNameMap.ContainsKey(name) ? this.entityNameMap[name] : null;
        }

        public IMetamodelEntity GetEntityByDtoName(string name)
        {
            name = name.ToLower();
            return this.dtoNameMap.ContainsKey(name) ? this.dtoNameMap[name] : null;
        }

        /// <summary>
        /// Registers one entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected IMetamodelEntity RegisterEntity(IMetamodelEntity entity)
        {
            if (this.entityMap.ContainsKey(entity.EntityType))
            {
                this.entityMap[entity.EntityType] = entity;
            }
            else
            {
                this.entityMap.Add(entity.EntityType, entity);
            }
            string name = entity.EntityType.Name.ToLower();
            if (this.entityNameMap.ContainsKey(name))
            {
                this.entityNameMap[name] = entity;
            }
            else
            {
                this.entityNameMap.Add(name, entity);
            }
            foreach (Type dtoType in entity.DtoTypes)
            {
                if (this.dtoMap.ContainsKey(dtoType))
                {
                    this.dtoMap[dtoType] = entity;
                }
                else
                {
                    this.dtoMap.Add(dtoType, entity);
                }
                string lowname = dtoType.Name.ToLower();
                if (this.dtoNameMap.ContainsKey(lowname))
                {
                    this.dtoNameMap[lowname] = entity;
                }
                else
                {
                    this.dtoNameMap.Add(lowname, entity);
                }
            }
            return entity;       
        }

        /// <summary>
        /// Registers a pair entity and dto.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dtoType">Type of the dto.</param>
        public IMetamodelEntity RegisterEntity(Type entityType, Type dtoType)
        {
            IMetamodelEntity metamodelEntity = this.GetEntity(entityType);
            if (metamodelEntity == null)
            {
                metamodelEntity = new MetamodelEntity(entityType);
                this.RegisterEntity(metamodelEntity);
            }
            metamodelEntity.AddDto(dtoType);
            if (this.dtoMap.ContainsKey(dtoType))
            {
                this.dtoMap[dtoType] = metamodelEntity;
            }
            else
            {
                this.dtoMap.Add(dtoType, metamodelEntity);
            }
            string lowname = dtoType.Name.ToLower();
            if (this.dtoNameMap.ContainsKey(lowname))
            {
                this.dtoNameMap[lowname] = metamodelEntity;
            }
            else
            {
                this.dtoNameMap.Add(lowname, metamodelEntity);
            }
            IObjectProxy proxy = ObjectProxyFactory.GetByType(dtoType);
            IList<string> sourceNames = new List<string>();
            IList<string> targetNames = new List<string>();
            foreach (PropertyInfo property in proxy.Properties)
            {
                DtoFieldAttribute attribute = property.GetCustomAttribute<DtoFieldAttribute>();
                if (attribute != null)
                {
                    metamodelEntity.AddFieldMap(dtoType, attribute.Name, property.Name);
                    sourceNames.Add(attribute.Name);
                    targetNames.Add(property.Name);
                }
            }
            
            ObjectProxyFactory.CreateMap(entityType, dtoType, sourceNames, targetNames);
            return metamodelEntity;
        }

        /// <summary>
        /// Sets the audit trail information for an entity.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="entId">The entity identifier.</param>
        /// <param name="auditType">Type of the audit.</param>
        public void SetAudit(string name, int entId, AuditType auditType)
        {
            IMetamodelEntity entity = this.GetEntityByName(name);
            if (entity != null)
            {
                entity.EntId = entId;
                entity.AuditType = auditType;
            }
        }

        #endregion
    }
}
