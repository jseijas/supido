using Supido.Business.Audit;
using Supido.Business.Context;
using Supido.Business.Filter;
using Supido.Business.Meta;
using Supido.Business.Query;
using Supido.Core.Container;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess;

namespace Supido.Business.BO
{
    /// <summary>
    /// Class for a business object based on a map between dto and entity using OpenAccess context.
    /// </summary>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public class ContextBO<TDto> : BaseBO<TDto>, IContextEntityBO
    {
        #region - Properties -

        /// <summary>
        /// Gets the context manager.
        /// </summary>
        /// <value>
        /// The context manager.
        /// </value>
        public IUserContext ContextManager { get; private set; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public OpenAccessContext Context
        {
            get { return this.ContextManager.Context; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic managed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic managed]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoManaged { get; set; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public IBOFilter Filter { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextBO{TDto}"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="autoManaged">if set to <c>true</c> [automatic managed].</param>
        public ContextBO(Type entityType, IUserContext contextManager, Type filterType, bool autoManaged = true)
            : base(entityType)
        {
            this.ContextManager = contextManager;
            this.AutoManaged = autoManaged;
            if (filterType == null)
            {
                this.Filter = null;
            }
            else
            {
                this.Filter = (IBOFilter)Activator.CreateInstance(filterType);
                this.Filter.Parent = this;
            }
        }

        #endregion

        #region - Methods -

        #region - Transaction Management Methods -

        /// <summary>
        /// Opens the transaction if the BO is automanaged.
        /// </summary>
        protected void Open()
        {
            if (this.AutoManaged)
            {
                this.ContextManager.Open();
            }
        }

        /// <summary>
        /// Closes the transaction if the BO is automanaged.
        /// </summary>
        protected void Close()
        {
            if (this.AutoManaged)
            {
                this.ContextManager.Close();
            }
        }

        /// <summary>
        /// Commits the transaction if the BO is automanaged.
        /// </summary>
        protected void Commit()
        {
            if (this.AutoManaged) 
            {
                this.ContextManager.Commit();
            }
        }

        /// <summary>
        /// Rollbacks the transaction if the BO is automanaged.
        /// </summary>
        protected void Rollback()
        {
            if (this.AutoManaged)
            {
                this.ContextManager.Rollback();
            }
        }

        #endregion

        #region - Protected Methods -

        /// <summary>
        /// Gets the base query.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable GetBaseQuery()
        {
            return this.Context.GetAll(this.EntityType.FullName);
        }

        #endregion

        #region - Methods from BaseBO -

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <param name="info">The query information.</param>
        /// <returns></returns>
        protected override IList GetEntityAll(QueryInfo info)
        {
            this.Open();
            try
            {
                dynamic query = this.GetBaseQuery();
                if (this.Filter != null)
                {
                    query = this.Filter.ApplySecurity(query);
                    query = this.Filter.ApplyFilter(query, info);
                    if (info != null)
                    {
                        if (info.SkipRecords != null)
                        {
                            query = query.Skip(info.SkipRecords.Value);
                        }
                        if (info.TakeRecords != null)
                        {
                            query = query.Take(info.TakeRecords.Value);
                        }
                    }
                }
                return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this.EntityType), query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Gets one entity by primary key.
        /// </summary>
        /// <param name="pk">The primary key.</param>
        /// <param name="info">The query information.</param>
        /// <returns></returns>
        protected override object GetEntityOne(string pk, QueryInfo info)
        {
            this.Open();
            try
            {
                if (!string.IsNullOrEmpty(pk)) 
                {
                    string[] keys = pk.Split('!');
                    int i = 0;
                    IMetamodelEntity helper = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(this.EntityType);
                    if (helper != null)
                    {
                        if (info == null)
                        {
                            info = new QueryInfo();
                        }
                        foreach (IMetamodelField field in helper.Fields)
                        {
                            if (field.IsPrimaryKey)
                            {
                                if (!string.IsNullOrEmpty(keys[i]))
                                {
                                    string dtoFieldName = helper.GetDtoFieldName(field.Name, typeof(TDto));
                                    info.Where(dtoFieldName, FacetOperation.Equal, keys[i]);
                                }
                                i++;
                            }
                        }
                    }
                }
                IList entities = this.GetEntityAll(info);
                if (entities.Count == 0)
                {
                    return null;
                }
                return entities[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override object InsertEntity(object entity)
        {
            this.Open();
            try
            {
                this.Context.Add(entity);
                this.Context.FlushChanges();
                this.ContextManager.Trail(TransacActionType.Insert, null, entity);
                this.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                this.Rollback();
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override object UpdateEntity(object entity)
        {
            this.Open();
            try
            {
                IMetamodelEntity helper = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(this.EntityType);
                object target = this.Context.GetObjectByKey(helper.GetObjectKey(entity));
                helper.FillUpdate(entity, target);
                this.Context.FlushChanges();
                this.Commit();
                return target;
            }
            catch (Exception ex)
            {
                this.Rollback();
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        protected override int DeleteEntity(string pk)
        {
            this.Open();
            try
            {
                IMetamodelEntity helper = IoC.Get<ISecurityManager>().MetamodelManager.GetEntity(this.EntityType);
                object entity = this.Context.GetObjectByKey(helper.GetObjectKeyByPk(pk));
                this.Context.Delete(entity);
                this.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                this.Rollback();
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        #endregion

        #endregion
    }
}
