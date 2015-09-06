using System;
using System.Collections.Generic;

namespace Supido.Core.Container
{
    /// <summary>
    /// Class for an Item of the IoC Container
    /// </summary>
    public class IoCItem
    {
        #region - Constants -

        /// <summary>
        /// The default instance name.
        /// </summary>
        public const string DefaultInstanceName = "default";

        #endregion

        #region - Fields -

        /// <summary>
        /// The instances of this item. If Per Call then is null. If Singleton then contains only one object.
        /// In the case of Per Token, contains one item for each token.
        /// </summary>
        private Dictionary<string, IoCInstance> instances = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the type of the interface.
        /// </summary>
        /// <value>
        /// The type of the interface.
        /// </value>
        public Type InterfaceType { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public Type ObjectType { get; private set; }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public IoCLifetime Lifetime { get; private set; }

        /// <summary>
        /// Gets or sets the expire mode.
        /// </summary>
        /// <value>
        /// The expire mode.
        /// </value>
        public IoCExpiration Expiration { get; set; }

        /// <summary>
        /// Gets or sets the expire time.
        /// </summary>
        /// <value>
        /// The expire time.
        /// </value>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the expire span.
        /// </summary>
        /// <value>
        /// The expire span.
        /// </value>
        public TimeSpan ExpireSpan { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCItem"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="lifetime">The lifetime.</param>
        public IoCItem(Type interfaceType, Type objectType, IoCLifetime lifetime)
        {
            this.InterfaceType = interfaceType;
            this.ObjectType = objectType;
            this.Lifetime = lifetime;
            this.Expiration = IoCExpiration.Never;
            this.ExpireTime = DateTime.UtcNow;
            this.ExpireSpan = TimeSpan.FromDays(1);
            if (this.Lifetime != IoCLifetime.PerCall)
            {
                this.instances = new Dictionary<string, IoCInstance>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCItem"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instance">The instance.</param>
        public IoCItem(Type interfaceType, object instance)
        {
            this.InterfaceType = interfaceType;
            this.ObjectType = instance.GetType();
            this.Lifetime = IoCLifetime.Singleton;
            this.Expiration = IoCExpiration.Never;
            this.ExpireTime = DateTime.UtcNow;
            this.ExpireSpan = TimeSpan.FromDays(1);
            this.instances = new Dictionary<string, IoCInstance>();
            this.instances.Add(DefaultInstanceName, new IoCInstance(instance));
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets a new instance of the object type.
        /// </summary>
        /// <returns></returns>
        protected object GetNewInstance()
        {
            return Activator.CreateInstance(ObjectType);
        }

        /// <summary>
        /// Gets the instance for a token from the dictionary, create a new one if not exists.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private object GetInstance(string token)
        {
            IoCInstance item;
            if (this.instances.ContainsKey(token))
            {
                item = this.instances[token];
            }
            else
            {
                item = new IoCInstance(this.GetNewInstance());
                this.instances.Add(token, item);
            }
            return item.Instance;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">IoC GetObject() called with invalid lifetime.</exception>
        public object GetObject()
        {
            switch (this.Lifetime)
            {
                case IoCLifetime.PerCall:
                    return this.GetNewInstance();
                case IoCLifetime.Singleton:
                    return this.GetInstance(DefaultInstanceName);
                default:
                    throw new ApplicationException("IoC GetObject() called with invalid lifetime.");
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public object GetObject(string token)
        {
            if (this.Lifetime == IoCLifetime.PerToken)
            {
                return this.GetInstance(token);
            }
            else
            {
                return this.GetObject();
            }
        }

        public void AddByToken(object instance, string token)
        {
            if (this.instances.ContainsKey(token))
            {
                this.instances.Remove(token);
            }
            IoCInstance item = new IoCInstance(instance);
            this.instances.Add(token, item);
        }

        /// <summary>
        /// Indicates if the token exists.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public bool ExistsToken(string token)
        {
            return this.instances.ContainsKey(token);
        }

        /// <summary>
        /// Expire the instance of the given token.
        /// </summary>
        /// <param name="token">The token.</param>
        public void ExpireToken(string token)
        {
            this.instances.Remove(token);
        }

        /// <summary>
        /// Checks the expiration.
        /// </summary>
        public void CheckExpiration()
        {
            if (this.Expiration == IoCExpiration.DateTime)
            {
                if (DateTime.UtcNow > this.ExpireTime.Add(this.ExpireSpan))
                {
                    this.instances.Clear();
                }
            }
            else if (this.Expiration == IoCExpiration.Idle)
            {
                List<string> toRemove = new List<string>();
                foreach (KeyValuePair<string, IoCInstance> kvp in this.instances)
                {
                    if (kvp.Value.IsIdle(this.ExpireSpan))
                    {
                        toRemove.Add(kvp.Key);
                    }
                }
                foreach (string key in toRemove)
                {
                    this.instances.Remove(key);
                }
            }
        }

        #endregion
    }
}
