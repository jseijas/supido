using Supido.Business.Audit;
using Supido.Business.BO;
using Supido.Business.Context;
using Supido.Business.DTO;
using Supido.Business.Meta;
using Supido.Business.Security;
using Supido.Business.Session;
using System;

namespace Supido.Business
{
    /// <summary>
    /// Interface for the security manager.
    /// </summary>
    public interface ISecurityManager
    {
        #region - Properties -

        /// <summary>
        /// Gets the session manager.
        /// </summary>
        /// <value>
        /// The session manager.
        /// </value>
        ISessionManager SessionManager { get; }

        /// <summary>
        /// Gets the audit manager.
        /// </summary>
        /// <value>
        /// The audit manager.
        /// </value>
        IAuditManager AuditManager { get; }

        /// <summary>
        /// Gets the metamodel manager.
        /// </summary>
        /// <value>
        /// The metamodel manager.
        /// </value>
        IMetamodelManager MetamodelManager { get; }

        /// <summary>
        /// Gets the business object manager.
        /// </summary>
        /// <value>
        /// The business object manager.
        /// </value>
        IBOManager BOManager { get; }

        /// <summary>
        /// Gets the type of the database context.
        /// </summary>
        /// <value>
        /// The type of the context.
        /// </value>
        Type ContextType { get; }

        /// <summary>
        /// Gets the scanner.
        /// </summary>
        /// <value>
        /// The scanner.
        /// </value>
        SecurityScanner Scanner { get; }

        #endregion

        #region - Methods -

        void Configure(string fileName);

        /// <summary>
        /// Logins a user into the system.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        IUserDto Login(string login, string password);

        /// <summary>
        /// Gets the user information from the session token.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        IUserDto GetUserInfo(string sessionToken);

        /// <summary>
        /// Logouts a session from the system.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        void Logout(string sessionToken);

        /// <summary>
        /// Gets the user context from a session token.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        IUserContext GetUserContext(string sessionToken);

        /// <summary>
        /// Gets a contextualized business object.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        ContextBO<TDto> GetBO<TDto>(string sessionToken);

        /// <summary>
        /// Gets a contextualized business object, dynamic way.
        /// </summary>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        object DynamicGetBO(Type dtoType, string sessionToken);

        #endregion
    }
}
