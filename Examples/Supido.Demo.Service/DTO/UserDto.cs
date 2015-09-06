using Supido.Business.DTO;
using System;

namespace Supido.Demo.Service.DTO
{
    /// <summary>
    /// Data transfer object for a user.
    /// </summary>
    public class UserDto : IUserDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long? UserId { get; set; }

        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        /// <value>
        /// The department identifier.
        /// </value>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public String Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        /// <value>
        /// The session token.
        /// </value>
        public string SessionToken { get; set; }

    }
}