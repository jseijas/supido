using Supido.Business.DTO;
using System;

namespace Supido.Demo.Service.DTO
{
    /// <summary>
    /// Data transfer object for a Session.
    /// </summary>
    public class SessionDto : ISessionDto
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        /// <value>
        /// The session token.
        /// </value>
        public string SessionToken { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the creation date time.
        /// </summary>
        /// <value>
        /// The creation date time.
        /// </value>
        public DateTime CreationDttm { get; set; }

        /// <summary>
        /// Gets or sets the last access date time.
        /// </summary>
        /// <value>
        /// The last access date time.
        /// </value>
        public DateTime LastAccessDttm { get; set; }

        #endregion
    }
}