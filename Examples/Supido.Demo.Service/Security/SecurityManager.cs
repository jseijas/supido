using Supido.Business.Security;
using Supido.Demo.Model;
using Supido.Demo.Service.DTO;
using System.Linq;
using Telerik.OpenAccess;

namespace Supido.Demo.Service.Security
{
    /// <summary>
    /// Class for the security manager
    /// </summary>
    public class SecurityManager : BaseSecurityManager
    {
        #region - constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityManager"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public SecurityManager()
            : base(typeof(EntitiesModel), typeof(UserDto), typeof(User))
        {        
        }

        #endregion

        #region - Methods from BaseSecurityManager -

        /// <summary>
        /// Gets a user by login and password.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        protected override object GetUserByLoginPass(OpenAccessContext context, string login, string password)
        {
            return context.GetAll<User>().Where(p => p.Email == login && p.Password == password).FirstOrDefault();
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        protected override object GetUserById(OpenAccessContext context, long userId)
        {
            return context.GetAll<User>().Where(p => p.UserId == userId).FirstOrDefault();
        }

        #endregion
    }
}