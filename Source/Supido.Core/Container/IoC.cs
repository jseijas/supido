using System;

namespace Supido.Core.Container
{
    /// <summary>
    /// Singleton for the default IoC container.
    /// </summary>
    public static class IoC
    {
        #region - Fields -

        /// <summary>
        /// The inner container instance.
        /// </summary>
        private static IoCContainer container = new IoCContainer();

        #endregion

        #region - Methods -

        /// <summary>
        /// Registers a singleton given the interface and the instance.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instance">The instance.</param>
        public static void Register(Type interfaceType, object instance)
        {
            container.RegisterSingleton(interfaceType, instance);
        }

        /// <summary>
        /// Registers a singleton given the interface and the instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        public static void Register<TInterface>(TInterface instance)
        {
            container.RegisterSingleton<TInterface>(instance);
        }

        /// <summary>
        /// Registers an instantiation type, given its interface, its type, the expiration mode, the span and the lifetime.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="expireMode">The expire mode.</param>
        /// <param name="span">The span.</param>
        /// <param name="lifetime">The lifetime.</param>
        public static void Register(Type interfaceType, Type objectType, IoCExpiration expireMode, TimeSpan span, IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            container.RegisterType(interfaceType, objectType, expireMode, span, lifetime);
        }

        /// <summary>
        /// Registers an instantiation type, given the interface, the type and the lifetime.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="lifetime">The lifetime.</param>
        public static void Register(Type interfaceType, Type objectType, IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            container.RegisterType(interfaceType, objectType, lifetime);
        }

        /// <summary>
        /// Registers an instantiation type, given the interface, the type and the lifetime.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        public static void Register<TInterface, TObject>(IoCLifetime lifetime = IoCLifetime.PerCall)
        {
            container.RegisterType<TInterface, TObject>(lifetime);
        }

        /// <summary>
        /// Registers a Per Token instantiation type, given the interface, the type and idle span
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="span">The span.</param>
        public static void Register<TInterface, TObject>(TimeSpan span)
        {
            container.RegisterType<TInterface, TObject>(span);
        }

        /// <summary>
        /// Registers a Per Token instance, given the interface, the instance and the token.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="token">The token.</param>
        public static void Register<TInterface>(TInterface instance, string token)
        {
            container.Register<TInterface>(instance, token);
        }

        /// <summary>
        /// Returns a container item given its interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        public static IoCItem GetInversionItem<TInterface>()
        {
            return container.GetInversionItem<TInterface>();
        }

        /// <summary>
        /// Gets the specified instance, given its interface type and the token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static T Get<T>(string token = IoCItem.DefaultInstanceName)
        {
            return container.Get<T>(token);
        }

        /// <summary>
        /// Gets the specified instance, given its interface type and the token
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static object Get(Type type, string token = IoCItem.DefaultInstanceName)
        {
            return container.Get(type, token);
        }

        /// <summary>
        /// Indicates if the token exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static bool ExistsToken<T>(string token)
        {
            return container.ExistsToken<T>(token);
        }

        #endregion
    }
}
