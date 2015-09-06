using System;

namespace Supido.Business.DTO
{
    /// <summary>
    /// Interface for the data of a session.
    /// </summary>
    public interface ISessionDto
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        /// <value>
        /// The session token.
        /// </value>
        string SessionToken { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        long UserId { get; set; }

        /// <summary>
        /// Gets or sets the creation date time.
        /// </summary>
        /// <value>
        /// The creation date time.
        /// </value>
        DateTime CreationDttm { get; set; }

        /// <summary>
        /// Gets or sets the last access date time.
        /// </summary>
        /// <value>
        /// The last access date time.
        /// </value>
        DateTime LastAccessDttm { get; set; }

        #endregion
    }
}
