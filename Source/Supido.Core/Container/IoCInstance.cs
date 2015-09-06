using System;

namespace Supido.Core.Container
{
    /// <summary>
    /// Envelope for an object instance.
    /// </summary>
    public class IoCInstance
    {
        #region - Fields -

        /// <summary>
        /// The instance of the object being enveloped.
        /// </summary>
        private object instance;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the last access.
        /// </summary>
        /// <value>
        /// The last access.
        /// </value>
        public DateTime LastAccess { get; set; }

        /// <summary>
        /// Gets the idle time.
        /// </summary>
        /// <value>
        /// The idle time.
        /// </value>
        public TimeSpan IdleTime
        {
            get
            {
                return DateTime.UtcNow.Subtract(this.LastAccess);
            }
        }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public object Instance
        {
            get
            {
                this.Use();
                return this.instance;
            }
            set
            {
                this.instance = value;
                this.Use();
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCInstance"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public IoCInstance(object instance)
        {
            this.Instance = instance;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Uses this instance, resetting idle.
        /// </summary>
        private void Use()
        {
            this.LastAccess = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether the specified span is idle.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <returns>
        ///   <c>true</c> if the specified span is idle; otherwise, <c>false</c>.
        /// </returns>
        public bool IsIdle(TimeSpan span)
        {
            return this.LastAccess.Add(span) < DateTime.UtcNow;
        }

        #endregion
    }
}
