using Supido.Business.Query;
using Supido.Core.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Supido.Business.BO
{
    /// <summary>
    /// Abstract class for a Business Object that is based on the mapping of a DTO to an entity.
    /// </summary>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public abstract class BaseBO<TDto> : IBO<TDto>, IEntityBO
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; set; }

        /// <summary>
        /// Gets the type of the dto.
        /// </summary>
        /// <value>
        /// The type of the dto.
        /// </value>
        public Type DtoType
        {
            get { return typeof(TDto); }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBO{TDto}"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public BaseBO(Type entityType)
        {
            this.EntityType = entityType;
        }

        #endregion

        #region - Events -

        /// <summary>
        /// Delegate for an event of the business object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="BOEventArgs{TDto}"/> instance containing the event data.</param>
        public delegate void BOEvent(IBO<TDto> sender, BOEventArgs<TDto> args);

        /// <summary>
        /// Occurs when [before get all].
        /// </summary>
        public event BOEvent BeforeGetAll;

        /// <summary>
        /// Occurs when [before get one].
        /// </summary>
        public event BOEvent BeforeGetOne;

        /// <summary>
        /// Occurs when [before insert].
        /// </summary>
        public event BOEvent BeforeInsert;

        /// <summary>
        /// Occurs when [before update].
        /// </summary>
        public event BOEvent BeforeUpdate;

        /// <summary>
        /// Occurs when [before delete].
        /// </summary>
        public event BOEvent BeforeDelete;

        /// <summary>
        /// Occurs when [after get all].
        /// </summary>
        public event BOEvent AfterGetAll;

        /// <summary>
        /// Occurs when [after get one].
        /// </summary>
        public event BOEvent AfterGetOne;

        /// <summary>
        /// Occurs when [after insert].
        /// </summary>
        public event BOEvent AfterInsert;

        /// <summary>
        /// Occurs when [after update].
        /// </summary>
        public event BOEvent AfterUpdate;

        /// <summary>
        /// Occurs when [after delete].
        /// </summary>
        public event BOEvent AfterDelete;

        #endregion

        #region - Methods -

        #region - Abstract Methods -

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <param name="info">The query information.</param>
        /// <returns></returns>
        protected abstract IList GetEntityAll(QueryInfo info);

        /// <summary>
        /// Gets one entity by primary key.
        /// </summary>
        /// <param name="pk">The primary key.</param>
        /// <param name="info">The query information.</param>
        /// <returns></returns>
        protected abstract object GetEntityOne(string pk, QueryInfo info);

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected abstract object InsertEntity(object entity);

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected abstract object UpdateEntity(object entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        protected abstract int DeleteEntity(string pk);

        #endregion

        #region - Methods from IBO -

        /// <summary>
        /// Gets all the DTOs.
        /// </summary>
        /// <returns></returns>
        public IList<TDto> GetAll()
        {
            if (this.BeforeGetAll != null)
            {
                this.BeforeGetAll(this, new BOEventArgs<TDto>());
            }
            IList<TDto> result = ObjectProxyFactory.MapToList<TDto>(this.GetEntityAll(null));
            if (this.AfterGetAll != null)
            {
                this.AfterGetAll(this, new BOEventArgs<TDto>(null, null, default(TDto), default(TDto), result, null));
            }
            return result;
        }

        /// <summary>
        /// Gets all the DTOs.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IList<TDto> GetAll(QueryInfo info)
        {
            if (this.BeforeGetAll != null)
            {
                this.BeforeGetAll(this, new BOEventArgs<TDto>(null, info, default(TDto), default(TDto), null, null));
            }
            IList<TDto> result = ObjectProxyFactory.MapToList<TDto>(this.GetEntityAll(info));
            if (this.AfterGetAll != null)
            {
                this.AfterGetAll(this, new BOEventArgs<TDto>(null, info, default(TDto), default(TDto), result, null));
            }
            return result;
        }

        /// <summary>
        /// Gets one DTO.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns></returns>
        public TDto GetOne(string pk)
        {
            if (this.BeforeGetOne != null)
            {
                this.BeforeGetOne(this, new BOEventArgs<TDto>(pk, null, default(TDto), default(TDto), null, null));
            }
            TDto result = ObjectProxyFactory.MapTo<TDto>(this.GetEntityOne(pk, null));
            if (this.AfterGetOne != null)
            {
                this.AfterGetOne(this, new BOEventArgs<TDto>(pk, null, default(TDto), result, null, null));
            }
            return result;
        }

        /// <summary>
        /// Gets one DTO.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <param name="info"></param>
        /// <returns></returns>
        public TDto GetOne(string pk, QueryInfo info)
        {
            if (this.BeforeGetOne != null)
            {
                this.BeforeGetOne(this, new BOEventArgs<TDto>(pk, info, default(TDto), default(TDto), null, null));
            }
            TDto result = ObjectProxyFactory.MapTo<TDto>(this.GetEntityOne(pk, info));
            if (this.AfterGetOne != null)
            {
                this.AfterGetOne(this, new BOEventArgs<TDto>(pk, info, default(TDto), result, null, null));
            }
            return result;
        }

        /// <summary>
        /// Inserts one DTO.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public TDto Insert(TDto src)
        {
            object entity = ObjectProxyFactory.MapTo(this.EntityType, src);
            if (this.BeforeInsert != null)
            {
                this.BeforeInsert(this, new BOEventArgs<TDto>(null, null, src, default(TDto), null, entity));
            }
            TDto result = ObjectProxyFactory.MapTo<TDto>(this.InsertEntity(entity));
            if (this.AfterInsert != null)
            {
                this.AfterInsert(this, new BOEventArgs<TDto>(null, null, src, result, null, entity));
            }
            return result;
        }

        /// <summary>
        /// Updates one DTO.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public TDto Update(TDto src)
        {
            object entity = ObjectProxyFactory.MapTo(this.EntityType, src);
            if (this.BeforeUpdate != null)
            {
                this.BeforeUpdate(this, new BOEventArgs<TDto>(null, null, src, default(TDto), null, entity));
            }
            TDto result = ObjectProxyFactory.MapTo<TDto>(this.UpdateEntity(entity));
            if (this.AfterUpdate != null)
            {
                this.AfterUpdate(this, new BOEventArgs<TDto>(null, null, src, result, null, entity));
            }
            return result;
        }

        /// <summary>
        /// Deletes one DTO by primary key.
        /// </summary>
        /// <param name="pk">The primary key.</param>
        /// <returns></returns>
        public int Delete(string pk)
        {
            if (this.BeforeDelete != null)
            {
                this.BeforeDelete(this, new BOEventArgs<TDto>(pk, null, default(TDto), default(TDto), null, null));
            }
            int result = this.DeleteEntity(pk);
            if (this.AfterDelete != null)
            {
                this.AfterDelete(this, new BOEventArgs<TDto>(pk, null, default(TDto), default(TDto), null, null));
            }
            return result;
        }

        #endregion

        #endregion
    }
}
