using Supido.Business.Session;
using Supido.Demo.Model;
using Supido.Demo.Service.DTO;
using System;
using System.Collections;
using System.Linq;
using Telerik.OpenAccess;

namespace Supido.Demo.Service.Security
{
    /// <summary>
    /// Class for the session manager
    /// </summary>
    public class SessionManager : BaseSessionManager
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        public SessionManager()
            : base(typeof(Session), typeof(SessionDto))
        {

        }

        #endregion

        #region - Methods -

        #region - Methods from BaseSessionManager -

        /// <summary>
        /// Gets a session by its session token.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        protected override object GetSessionByToken(OpenAccessContext context, string sessionToken)
        {
            return context.GetAll<Session>().Where(p => p.SessionToken == sessionToken).FirstOrDefault();
        }

        /// <summary>
        /// Gets all sessions of user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        protected override IList GetAllSessionsOfUser(OpenAccessContext context, long userId)
        {
            return context.GetAll<Session>().Where(p => p.UserId == userId).ToList();
        }

        /// <summary>
        /// Creates a new session object.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        protected override object NewSession(long userId, string sessionToken)
        {
            Session session = new Session();
            session.SessionToken = sessionToken;
            session.UserId = Convert.ToInt32(userId);
            session.CreationDttm = DateTime.UtcNow;
            session.UpdateDttm = DateTime.UtcNow;
            return session;

        }

        /// <summary>
        /// Updates the session last access.
        /// </summary>
        /// <param name="session">The session.</param>
        protected override void UpdateSessionAccess(object session)
        {
            (session as Session).UpdateDttm = DateTime.UtcNow;
        }

        #endregion

        #endregion
    }
}