using System;
using System.Collections.Generic;

namespace Supido.Core.Container
{
    /// <summary>
    /// Inversion container.
    /// </summary>
    public class IoCContainer
    {
        #region - Fields -

        /// <summary>
        /// The items contained by the container.
        /// </summary>
        private Dictionary<Type, IoCItem> items = new Dictionary<Type, IoCItem>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="expireMode">The expire mode.</param>
        /// <param name="span">The span.</param>
        /// <param name="lifetime">The lifetime.</param>
        public void RegisterType(Type interfaceType, Type objectType, IoCExpiration expireMode, TimeSpan span,
            IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            if (items.ContainsKey(interfaceType))
            {
                items.Remove(interfaceType);
            }
            IoCItem item = new IoCItem(interfaceType, objectType, lifetime);
            item.Expiration = expireMode;
            item.ExpireSpan = span;
            items.Add(interfaceType, item);
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="lifetime">The lifetime.</param>
        public void RegisterType(Type interfaceType, Type objectType, IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            this.RegisterType(interfaceType, objectType, IoCExpiration.Never, TimeSpan.FromMinutes(5), lifetime);
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        public void RegisterType<TInterface, TObject>(IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            RegisterType(typeof(TInterface), typeof(TObject), lifetime);
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="span">The span.</param>
        public void RegisterType<TInterface, TObject>(TimeSpan span)
        {
            RegisterType(typeof(TInterface), typeof(TObject), IoCExpiration.Idle, span, IoCLifetime.PerToken);
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterSingleton<TInterface>(TInterface instance)
        {
            if (items.ContainsKey(typeof(TInterface)))
            {
                items.Remove(typeof(TInterface));
            }
            items.Add(typeof(TInterface), new IoCItem(typeof(TInterface), instance));
        }

        public void RegisterSingleton(Type interfaceType, object instance)
        {
            if (items.ContainsKey(interfaceType))
            {
                items.Remove(interfaceType);
            }
            items.Add(interfaceType, new IoCItem(interfaceType, instance));
        }

        public void Register(Type interfaceType, object instance, string token)
        {
            IoCItem item;
            if (!items.ContainsKey(interfaceType))
            {
                item = new IoCItem(interfaceType, instance.GetType(), IoCLifetime.PerToken);
                items.Add(interfaceType, item);
            }
            else
            {
                item = items[interfaceType];
            }
            item.AddByToken(instance, token);
        }

        public void Register<TInstance>(object instance, string token)
        {
            this.Register(typeof(TInstance), instance, token);
        }

        /// <summary>
        /// Gets the inversion item.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        public IoCItem GetInversionItem<TInterface>()
        {
            if (this.items.ContainsKey(typeof(TInterface)))
            {
                return this.items[typeof(TInterface)];
            }
            return null;
        }

        /// <summary>
        /// Gets the specified token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public T Get<T>(string token = IoCItem.DefaultInstanceName)
        {
            this.CheckExpiration();
            if (items.ContainsKey(typeof(T)))
            {
                return (T)items[typeof(T)].GetObject(token);
            }
            return default(T);
        }

        /// <summary>
        /// Gets the specified instance of a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public object Get(Type type, string token = IoCItem.DefaultInstanceName)
        {
            this.CheckExpiration();
            if (items.ContainsKey(type))
            {
                return items[type].GetObject(token);
            }
            return null;
        }

        /// <summary>
        /// Indicates if the token exists at the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public bool ExistsToken<T>(string token)
        {
            this.CheckExpiration();
            if (items.ContainsKey(typeof(T)))
            {
                return items[typeof(T)].ExistsToken(token);
            }
            return false;
        }

        /// <summary>
        /// Checks the expiration.
        /// </summary>
        public void CheckExpiration()
        {
            foreach (KeyValuePair<Type, IoCItem> kvp in this.items)
            {
                kvp.Value.CheckExpiration();
            }
        }

        #endregion
    }
}
