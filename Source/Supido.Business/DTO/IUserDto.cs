
namespace Supido.Business.DTO
{
    /// <summary>
    /// Interface for a user data transfer object
    /// </summary>
    public interface IUserDto
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        long? UserId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        /// <value>
        /// The session token.
        /// </value>
        string SessionToken { get; set; }

        #endregion
    }
}
