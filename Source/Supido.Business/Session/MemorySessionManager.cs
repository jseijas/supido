using Supido.Business.DTO;
using System;
using System.Collections.Generic;
using Telerik.OpenAccess;

namespace Supido.Business.Session
{
    /// <summary>
    /// In memory session manager
    /// </summary>
    public class MemorySessionManager : ISessionManager
    {
        #region - Fields -

        /// <summary>
        /// The sessions by token map
        /// </summary>
        private Dictionary<string, MemorySessionDto> sessionsByToken = new Dictionary<string, MemorySessionDto>();

        /// <summary>
        /// The sessions by user map
        /// </summary>
        private Dictionary<long, MemorySessionDto> sessionsByUser = new Dictionary<long, MemorySessionDto>();

        #endregion

        #region - Constructors -

        #endregion

        #region - Methods from BaseSessionManager -

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
            if (this.sessionsByToken.ContainsKey(sessionToken))
            {
                this.sessionsByToken.Remove(sessionToken);
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
            if (this.sessionsByUser.ContainsKey(userId))
            {
                MemorySessionDto session = this.sessionsByUser[userId];
                this.sessionsByUser.Remove(userId);
                this.sessionsByToken.Remove(session.SessionToken);
            }
            MemorySessionDto newsession = new MemorySessionDto();
            newsession.SessionToken = sessionToken;
            newsession.UserId = userId;
            newsession.CreationDttm = DateTime.UtcNow;
            newsession.LastAccessDttm = DateTime.UtcNow;
            this.sessionsByToken.Add(sessionToken, newsession);
            this.sessionsByUser.Add(userId, newsession);
        }

        /// <summary>
        /// Gets the session data.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        public ISessionDto GetSessionData(OpenAccessContext context, string sessionToken)
        {
            if (this.sessionsByToken.ContainsKey(sessionToken))
            {
                return this.sessionsByToken[sessionToken];
            }
            return null;
        }

        /// <summary>
        /// Updates the session.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        public void UpdateSession(OpenAccessContext context, string sessionToken)
        {
            if (this.sessionsByToken.ContainsKey(sessionToken))
            {
                this.sessionsByToken[sessionToken].LastAccessDttm = DateTime.UtcNow;
            }
        }

        #endregion
    }
}
