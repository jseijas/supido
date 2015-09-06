using Supido.Business.BO;
using Supido.Business.DTO;
using Supido.Core.Container;
using System;
using Telerik.OpenAccess;

namespace Supido.Business.Context
{
    /// <summary>
    /// Class for a User Context
    /// </summary>
    public class UserContext : IUserContext, IDisposable
    {
        #region - Properties -

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public OpenAccessContext Context { get; private set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public IUserDto User { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserContext(IUserDto user)
        {
            this.User = user;
        }

        #endregion

        #region - Methods -

        #region - Methods from IUserContext -

        /// <summary>
        /// Opens a new transaction.
        /// </summary>
        public void Open()
        {
            this.Close();
            this.Context = (OpenAccessContext)Activator.CreateInstance(IoC.Get<ISecurityManager>().ContextType);
            // TODO: start transaction
            //IoC.Get<ISecurityManager>().AuditManager.StartTransaction(this)
        }

        /// <summary>
        /// Closes the current transaction.
        /// </summary>
        public void Close()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
                this.Context = null;
            }
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void Commit()
        {
            // TODO: end transaction
            // IoC.Get<ISecurityManager>().AuditManager.EndTr...
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Rollbacks the current transaction.
        /// </summary>
        public void Rollback()
        {
            this.Context.ClearChanges();
        }

        /// <summary>
        /// Creates a new BO for the given DTO type.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="automanaged">if set to <c>true</c> [automanaged].</param>
        /// <returns></returns>
        public ContextBO<TDto> NewBO<TDto>(bool automanaged = true)
        {
            IBOManager boManager = IoC.Get<ISecurityManager>().BOManager;
            Type filterType = boManager.GetFilterType<TDto>();
            Type boType = boManager.GetBOType<TDto>();
            Type entityType = IoC.Get<ISecurityManager>().MetamodelManager.GetEntityByDto<TDto>().EntityType;
            return (ContextBO<TDto>)Activator.CreateInstance(boType, entityType, this, filterType, automanaged);
        }

        #endregion

        #region - Methods from IDisposable -

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }

        #endregion

        #endregion
    }
}
