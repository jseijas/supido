using System;
using System.Collections.Generic;
using Telerik.OpenAccess;

namespace Supido.Business.Meta
{
    /// <summary>
    /// Interface for a metamodel entity
    /// </summary>
    public interface IMetamodelEntity
    {
        #region - Properties -

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        Type EntityType { get; }

        /// <summary>
        /// Gets the list of DTO types for this entity. So an entity has a 1-n relationship with several DTOs.
        /// </summary>
        /// <value>
        /// The dto types.
        /// </value>
        IList<Type> DtoTypes { get; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        IList<IMetamodelField> Fields { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a field information.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isPrimaryKey">if set to <c>true</c> [is primary key].</param>
        /// <param name="avoidUpdate">if set to <c>true</c> [avoid update].</param>
        /// <returns></returns>
        IMetamodelField AddField(string name, bool isPrimaryKey, bool avoidUpdate);

        /// <summary>
        /// Gets the object key by pk.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        ObjectKey GetObjectKeyByPk(string pk);

        /// <summary>
        /// Gets the object key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        ObjectKey GetObjectKey(object entity);

        /// <summary>
        /// Gets the string key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        string GetStringKey(object entity);

        /// <summary>
        /// Fills target object from source object, taking into account th fields to be avoided.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        void FillUpdate(object source, object target);

        /// <summary>
        /// Adds a field mapping for a given DTO type.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="fieldEntityName">Name of the field entity.</param>
        /// <param name="fieldDtoName">Name of the field dto.</param>
        void AddFieldMap(Type dtoType, string fieldEntityName, string fieldDtoName);

        /// <summary>
        /// Gets the entity field name from the DTO type and the dto field name.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="dtoFieldName">Name of the dto field.</param>
        /// <returns></returns>
        string GetEntityFieldName(Type dtoType, string dtoFieldName);

        /// <summary>
        /// Gets the dto field name from the DTO type and the entity field name.
        /// </summary>
        /// <param name="entityFieldName">Name of the entity field.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        string GetDtoFieldName(string entityFieldName, Type dtoType);

        /// <summary>
        /// Determines whether the entity has a relationship with a given DTO.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        bool ContainsDto(Type dtoType);

        /// <summary>
        /// Adds a DTO type for the entity.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        void AddDto(Type dtoType);

        #endregion
    }
}
