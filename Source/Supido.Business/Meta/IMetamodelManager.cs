using Supido.Business.Audit;
using System;

namespace Supido.Business.Meta
{
    /// <summary>
    /// Interface for a metamodel manager
    /// </summary>
    public interface IMetamodelManager
    {
        #region - Methods -

        /// <summary>
        /// Gets the entity metamodel by entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IMetamodelEntity GetEntity<TEntity>();

        /// <summary>
        /// Gets the entity metamodel by entity type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IMetamodelEntity GetEntity(Type type);

        /// <summary>
        /// Gets the entity metamodel by dto type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <returns></returns>
        IMetamodelEntity GetEntityByDto<TDto>();

        /// <summary>
        /// Gets the entity metamodel by dto type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IMetamodelEntity GetEntityByDto(Type type);

        /// <summary>
        /// Gets the entity metamodel by entity name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IMetamodelEntity GetEntityByName(string name);

        /// <summary>
        /// Gets the entity metamodel by the dto name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IMetamodelEntity GetEntityByDtoName(string name);

        /// <summary>
        /// Registers the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        IMetamodelEntity RegisterEntity(Type entityType, Type dtoType);

        /// <summary>
        /// Sets the audit trail information for an entity.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="entId">The entity identifier.</param>
        /// <param name="auditType">Type of the audit.</param>
        void SetAudit(string name, int entId, AuditType auditType);

        #endregion
    }
}
