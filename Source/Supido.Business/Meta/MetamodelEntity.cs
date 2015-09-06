using Supido.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.OpenAccess;

namespace Supido.Business.Meta
{
    /// <summary>
    /// Class for a metmodel entity
    /// </summary>
    public class MetamodelEntity : IMetamodelEntity
    {
        #region - Fields -

        /// <summary>
        /// Maps from entity field name to dto field names.
        /// </summary>
        private Dictionary<string, Dictionary<Type, string>> entityFieldMap = new Dictionary<string, Dictionary<Type, string>>();

        /// <summary>
        /// Maps from dto field names to entity names.
        /// </summary>
        private Dictionary<Type, Dictionary<string, string>> dtoFieldMap = new Dictionary<Type, Dictionary<string, string>>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the list of DTO types for this entity. So an entity has a 1-n relationship with several DTOs.
        /// </summary>
        /// <value>
        /// The dto types.
        /// </value>
        public IList<Type> DtoTypes { get; private set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IList<IMetamodelField> Fields { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MetamodelEntity"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public MetamodelEntity(Type entityType)
        {
            this.EntityType = entityType;
            this.DtoTypes = new List<Type>();
            this.Fields = new List<IMetamodelField>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetamodelEntity"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dtoType">Type of the dto.</param>
        public MetamodelEntity(Type entityType, Type dtoType)
            : this(entityType)
        {
            this.DtoTypes.Add(dtoType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetamodelEntity"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dtoTypes">The dto types.</param>
        public MetamodelEntity(Type entityType, IList<Type> dtoTypes)
            : this(entityType)
        {
            (this.DtoTypes as List<Type>).AddRange(dtoTypes);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a field information.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isPrimaryKey">if set to <c>true</c> [is primary key].</param>
        /// <param name="avoidUpdate">if set to <c>true</c> [avoid update].</param>
        /// <returns></returns>
        public IMetamodelField AddField(string name, bool isPrimaryKey, bool avoidUpdate)
        {
            IMetamodelField field = new MetamodelField(name, isPrimaryKey, avoidUpdate);
            this.Fields.Add(field);
            return field;
        }

        /// <summary>
        /// Changes the type of a string value to a target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        private object ChangeType(string value, Type targetType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return Convert.ChangeType(value, underlyingType);
            }
            else
            {
                return Convert.ChangeType(value, targetType);
            }
        }

        /// <summary>
        /// Gets the object key by pk.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        public ObjectKey GetObjectKeyByPk(string pk)
        {
            string[] tokens = pk.Split('!');
            if (tokens.Length == 1)
            {
                return new ObjectKey(this.EntityType.Name, pk);
            }
            else
            {
                List<KeyValuePair<string, object>> keyPairs = new List<KeyValuePair<string, object>>();
                int i = 0;
                IObjectProxy proxy = ObjectProxyFactory.GetByType(this.EntityType);
                foreach (IMetamodelField field in this.Fields)
                {
                    if (field.IsPrimaryKey)
                    {
                        keyPairs.Add(new KeyValuePair<string, object>(field.Name, this.ChangeType(tokens[i], proxy.GetPropertyType(field.Name))));
                        i++;
                    }
                }
                return new ObjectKey(this.EntityType.Name, keyPairs);
            }
        }

        /// <summary>
        /// Gets the object key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public ObjectKey GetObjectKey(object entity)
        {
            List<KeyValuePair<string, object>> keyPairs = new List<KeyValuePair<string, object>>();
            IObjectProxy proxy = ObjectProxyFactory.GetByType(this.EntityType);
            foreach (IMetamodelField field in this.Fields)
            {
                if (field.IsPrimaryKey)
                {
                    keyPairs.Add(new KeyValuePair<string, object>(field.Name, proxy.GetValue(entity, field.Name)));
                }
            }
            return new ObjectKey(this.EntityType.Name, keyPairs);
        }

        /// <summary>
        /// Gets the string key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public string GetStringKey(object entity)
        {
            IObjectProxy proxy = ObjectProxyFactory.GetByType(this.EntityType);
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (IMetamodelField field in this.Fields)
            {
                if (field.IsPrimaryKey)
                {
                    if (i > 0)
                    {
                        sb.Append("!");
                    }
                    sb.Append(proxy.GetValue(entity, field.Name).ToString());
                    i++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Fills target object from source object, taking into account th fields to be avoided.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public void FillUpdate(object source, object target)
        {
            IObjectProxy proxy = ObjectProxyFactory.GetByType(this.EntityType);
            foreach (IMetamodelField field in this.Fields)
            {
                if (!field.AvoidUpdate)
                {
                    proxy.SetValue(target, field.Name, proxy.GetValue(source, field.Name));
                }
            }
        }

        /// <summary>
        /// Adds a field mapping for a given DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="fieldEntityName">Name of the field entity.</param>
        /// <param name="fieldDtoName">Name of the field dto.</param>
        public void AddFieldMap(Type dtoType, string fieldEntityName, string fieldDtoName) 
        {
            // maps entity -> dto
            if (!entityFieldMap.ContainsKey(fieldEntityName))
            {
                this.entityFieldMap.Add(fieldEntityName, new Dictionary<Type, string>());
            }
            Dictionary<Type, string> dtoMap = this.entityFieldMap[fieldEntityName];
            if (dtoMap.ContainsKey(dtoType))
            {
                dtoMap[dtoType] = fieldDtoName;
            }
            else
            {
                dtoMap.Add(dtoType, fieldDtoName);
            }
            // maps dto -> entity
            if (!dtoFieldMap.ContainsKey(dtoType))
            {
                this.dtoFieldMap.Add(dtoType, new Dictionary<string, string>());
            }
            Dictionary<string, string> nameMap = this.dtoFieldMap[dtoType];
            if (nameMap.ContainsKey(fieldDtoName))
            {
                nameMap[fieldDtoName] = fieldEntityName;
            }
            else
            {
                nameMap.Add(fieldDtoName, fieldEntityName);
            }
        }

        /// <summary>
        /// Gets the entity field name from the DTO type and the dto field name.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="dtoFieldName">Name of the dto field.</param>
        /// <returns></returns>
        public string GetEntityFieldName(Type dtoType, string dtoFieldName)
        {
            if (this.dtoFieldMap.ContainsKey(dtoType))
            {
                Dictionary<string, string> nameMap = this.dtoFieldMap[dtoType];
                if (nameMap.ContainsKey(dtoFieldName))
                {
                    return nameMap[dtoFieldName];
                }
            }
            return dtoFieldName;
        }

        /// <summary>
        /// Gets the dto field name from the DTO type and the entity field name.
        /// </summary>
        /// <param name="entityFieldName">Name of the entity field.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        public string GetDtoFieldName(string entityFieldName, Type dtoType)
        {
            if (this.entityFieldMap.ContainsKey(entityFieldName))
            {
                Dictionary<Type, string> dtoMap = this.entityFieldMap[entityFieldName];
                if (dtoMap.ContainsKey(dtoType))
                {
                    return dtoMap[dtoType];
                }
            }
            return entityFieldName;
        }

        /// <summary>
        /// Determines whether the entity has a relationship with a given DTO.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        public bool ContainsDto(Type dtoType)
        {
            return this.DtoTypes.IndexOf(dtoType) > -1;
        }

        /// <summary>
        /// Adds a DTO type for the entity.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        public void AddDto(Type dtoType)
        {
            if (!this.ContainsDto(dtoType))
            {
                this.DtoTypes.Add(dtoType);
            }
        }

        #endregion
    }
}
