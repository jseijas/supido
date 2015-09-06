using Supido.Business.DTO;
using Telerik.OpenAccess;

namespace Supido.Business.Session
{
    /// <summary>
    /// Interface for a session manager, that takes care of the user sessions of the application.
    /// </summary>
    public interface ISessionManager
    {
        #region - Methods -

        /// <summary>
        /// Creates a new session token.
        /// </summary>
        /// <returns>New session token.</returns>
        string NewSessionToken();

        /// <summary>
        /// Removes the session.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="sessionToken">The session token.</param>
        void RemoveSession(OpenAccessContext context, string sessionToken);

        /// <summary>
        /// Adds the session for user.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionToken">The session token.</param>
        void AddSessionForUser(OpenAccessContext context, long userId, string sessionToken);

        /// <summary>
        /// Gets the session data.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        ISessionDto GetSessionData(OpenAccessContext context, string sessionToken);

        /// <summary>
        /// Updates the session.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        void UpdateSession(OpenAccessContext context, string sessionToken);

        #endregion
    }
}
