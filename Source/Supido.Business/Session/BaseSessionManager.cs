using Supido.Business.DTO;
using Supido.Core.Proxy;
using System;
using System.Collections;
using Telerik.OpenAccess;

namespace Supido.Business.Session
{
    /// <summary>
    /// Abstract class for helping in the process of define our session manager.
    /// </summary>
    public abstract class BaseSessionManager : ISessionManager
    {
        #region - Fields -

        /// <summary>
        /// Type of the DTO for the session data.
        /// </summary>
        private Type sessionDtoType;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSessionManager"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="sessionDtoType">Type of the session dto.</param>
        public BaseSessionManager(Type entityType, Type sessionDtoType)
        {
            this.sessionDtoType = sessionDtoType;
            ObjectProxyFactory.CreateMap(entityType, this.sessionDtoType);
        }

        #endregion

        #region - Methods -

        #region - Abstract Methods -

        /// <summary>
        /// Gets a session persistence object from its session token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        protected abstract object GetSessionByToken(OpenAccessContext context, string sessionToken);

        /// <summary>
        /// Gets all database sessions for a user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        protected abstract IList GetAllSessionsOfUser(OpenAccessContext context, long userId);

        /// <summary>
        /// Creates a new session object.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        protected abstract object NewSession(long userId, string sessionToken);

        /// <summary>
        /// Updates the session last access date time.
        /// </summary>
        /// <param name="session">The session.</param>
        protected abstract void UpdateSessionAccess(object session);

        #endregion

        #region - Protected Methods -

        /// <summary>
        /// Removes all the sessions of an user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        protected void RemoveSessionsOfUser(OpenAccessContext context, long userId)
        {
            IList sessions = this.GetAllSessionsOfUser(context, userId);
            if (sessions != null)
            {
                foreach (object session in sessions)
                {
                    context.Delete(session);
                }
            }
        }

        #endregion

        #region - Methods from ISessionManager -

        /// <summary>
        /// Creates a new session token.
        /// </summary>
        /// <returns>
        /// New session token.
        /// </returns>
        public string NewSessionToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// Removes the session.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="sessionToken">The session token.</param>
        public void RemoveSession(OpenAccessContext context, string sessionToken)
        {
            object session = this.GetSessionByToken(context, sessionToken);
            if (session != null)
            {
                context.Delete(session);
            }
        }

        /// <summary>
        /// Adds the session for user.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionToken">The session token.</param>
        public void AddSessionForUser(OpenAccessContext context, long userId, string sessionToken)
        {
            this.RemoveSessionsOfUser(context, userId);
            object session = this.NewSession(userId, sessionToken);
            context.Add(session);
        }

        /// <summary>
        /// Gets the session data.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ISessionDto GetSessionData(OpenAccessContext context, string sessionToken)
        {
            object session = this.GetSessionByToken(context, sessionToken);
            return session == null ? null : (ISessionDto)ObjectProxyFactory.MapTo(this.sessionDtoType, session);
        }

        /// <summary>
        /// Updates the session.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        public void UpdateSession(OpenAccessContext context, string sessionToken)
        {
            object session = this.GetSessionByToken(context, sessionToken);
            if (session != null)
            {
                this.UpdateSessionAccess(session);
            }
        }

        #endregion

        #endregion
    }
}
