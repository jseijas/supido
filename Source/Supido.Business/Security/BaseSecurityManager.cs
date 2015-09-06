using Supido.Business.BO;
using Supido.Business.Context;
using Supido.Business.DTO;
using Supido.Business.Meta;
using Supido.Business.Session;
using Supido.Core.Container;
using Supido.Core.Proxy;
using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace Supido.Business.Security
{
    /// <summary>
    /// Abstract class for a Security Manager
    /// </summary>
    public abstract class BaseSecurityManager : ISecurityManager
    {
        #region - Fields -

        private Dictionary<Type, MethodInfo> boMethods = new Dictionary<Type, MethodInfo>();

        private MethodInfo getBoMethod = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the session manager.
        /// </summary>
        /// <value>
        /// The session manager.
        /// </value>
        public ISessionManager SessionManager { get; private set; }

        /// <summary>
        /// Gets the business object manager.
        /// </summary>
        /// <value>
        /// The business object manager.
        /// </value>
        public IBOManager BOManager { get; private set; }

        /// <summary>
        /// Gets the metamodel manager.
        /// </summary>
        /// <value>
        /// The metamodel manager.
        /// </value>
        public IMetamodelManager MetamodelManager { get; private set; }

        /// <summary>
        /// Gets the type of the user dto.
        /// </summary>
        /// <value>
        /// The type of the user dto.
        /// </value>
        public Type UserDtoType { get; private set; }

        /// <summary>
        /// Gets the type of the context.
        /// </summary>
        /// <value>
        /// The type of the context.
        /// </value>
        public Type ContextType { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSecurityManager"/> class.
        /// </summary>
        /// <param name="contextType">Type of the context.</param>
        /// <param name="userDtoType">Type of the user dto.</param>
        /// <param name="fileName">Name of the file.</param>
        public BaseSecurityManager(Type contextType, Type userDtoType, Type entityType, string fileName)
        {
            this.ContextType = contextType;
            this.UserDtoType = userDtoType;
            ObjectProxyFactory.CreateMap(userDtoType, entityType);

            this.SessionManager = IoC.Get<ISessionManager>();
            this.BOManager = new BOManager();
            this.MetamodelManager = new MetamodelManager();
            this.Configure(fileName);
        }

        #endregion

        #region - Methods -

        #region - Protected Methods -

        /// <summary>
        /// Gets a user by login and password.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        protected abstract object GetUserByLoginPass(OpenAccessContext context, string login, string password);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        protected abstract object GetUserById(OpenAccessContext context, long userId);

        /// <summary>
        /// Gets the open access metadata container.
        /// </summary>
        /// <returns></returns>
        private MetadataContainer GetOpenAccessMetadata()
        {
            using (OpenAccessContext context = this.NewContext())
            {
                return context.Metadata;
            }
            
        }

        /// <summary>
        /// Gets the meta tables.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private Dictionary<string, MetaPersistentType> GetMetaTables(MetadataContainer container) 
        {
            Dictionary<string, MetaPersistentType> result = new Dictionary<string, MetaPersistentType>();
            foreach (MetaPersistentType type in container.PersistentTypes)
            {
                result.Add(type.FullName, type);

            }
            return result;
        }

        /// <summary>
        /// Configures the entities.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="metatables">The metatables.</param>
        protected void ConfigureEntities(XmlNode rootNode, Dictionary<string, MetaPersistentType> metatables)
        {
            NodeAttributes rootAtributes = new NodeAttributes(rootNode);
            bool autoscan = rootAtributes.AsBool("autoscan", true);
            string entityPreffix = rootAtributes.AsString("entityPreffix");
            if (autoscan)
            {
                SecurityScanner scanner = new SecurityScanner(this);
                scanner.EntityPreffix = rootAtributes.AsString("entityPreffix");
                scanner.EntitySuffix = rootAtributes.AsString("entitySuffix");
                scanner.DtoPreffix = rootAtributes.AsString("dtoPreffix");
                scanner.DtoSuffix = rootAtributes.AsString("dtoSuffix", "Dto");
                scanner.FilterPreffix = rootAtributes.AsString("filterPreffix");
                scanner.FilterSuffix = rootAtributes.AsString("filterSuffix", "Filter");
                scanner.BOPreffix = rootAtributes.AsString("boPreffix");
                scanner.BOSuffix = rootAtributes.AsString("boSuffix", "BO");
                scanner.Metatables = metatables;
                scanner.ScanNamespace(rootAtributes.AsString("namespaces"));
            }
        }

        /// <summary>
        /// Configures the Security Manager from a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected virtual void Configure(string fileName)
        {
            MetadataContainer metadata = this.GetOpenAccessMetadata();
            Dictionary<string, MetaPersistentType> metatables = this.GetMetaTables(metadata);
            XmlDocument document = null;
            if (!string.IsNullOrEmpty(fileName))
            {
                document = XmlResources.GetFromResource(fileName);
                if (document == null)
                {
                    document = XmlResources.GetFromEmbeddedResource(fileName);
                }
            }
            if (document != null)
            {
                ObjectProxyFactory.ConfigureMappers(document);
                XmlNode rootNode = document.SelectSingleNode("configuration/entities");
                if (rootNode != null)
                {
                    this.ConfigureEntities(rootNode, metatables);
                }
            }
            else
            {
                SecurityScanner scanner = new SecurityScanner(this);
                scanner.EntityPreffix = string.Empty;
                scanner.EntitySuffix = string.Empty;
                scanner.DtoPreffix = string.Empty;
                scanner.DtoSuffix = "Dto";
                scanner.FilterPreffix = string.Empty;
                scanner.FilterSuffix = "Filter";
                scanner.BOPreffix = string.Empty;
                scanner.BOSuffix = "BO";
                scanner.Metatables = metatables;
                scanner.ScanNamespace(string.Empty);
            }
        }

        /// <summary>
        /// Creates a new Context.
        /// </summary>
        /// <returns></returns>
        protected virtual OpenAccessContext NewContext()
        {
            return (OpenAccessContext)Activator.CreateInstance(this.ContextType);
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        protected virtual IUserDto GetUserInfo(OpenAccessContext context, string sessionToken) 
        {
            ISessionDto sessionData = this.SessionManager.GetSessionData(context, sessionToken);
            if (sessionData == null)
            {
                return null;
            }
            object user = this.GetUserById(context, sessionData.UserId);
            if (user == null)
            {
                this.SessionManager.RemoveSession(context, sessionToken);
                context.SaveChanges();
                return null;
            }
            IUserDto result = (IUserDto)ObjectProxyFactory.MapTo(this.UserDtoType, user);
            result.Password = null;
            result.SessionToken = sessionData.SessionToken;
            return result;
        }


        #endregion

        #region - Methods from ISecurityManager -

        /// <summary>
        /// Logins a user into the system.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Login invalid</exception>
        public IUserDto Login(string login, string password)
        {
            using (OpenAccessContext context = this.NewContext())
            {
                object user = this.GetUserByLoginPass(context, login, password);
                if (user == null)
                {
                    throw new ArgumentException("Login invalid");
                }
                IUserDto result = (IUserDto)ObjectProxyFactory.MapTo(this.UserDtoType, user);
                result.Password = null;
                result.SessionToken = this.SessionManager.NewSessionToken();
                this.SessionManager.AddSessionForUser(context, result.UserId.Value, result.SessionToken);
                context.SaveChanges();
                return result;
            }

        }

        /// <summary>
        /// Gets the user information from the session token.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        public IUserDto GetUserInfo(string sessionToken)
        {
            using (OpenAccessContext context = this.NewContext())
            {
                return this.GetUserInfo(context, sessionToken);
            }
        }

        /// <summary>
        /// Logouts a session from the system.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Logout(string sessionToken)
        {
            using (OpenAccessContext context = this.NewContext())
            {
                this.SessionManager.RemoveSession(context, sessionToken);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the user context from a session token.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IUserContext GetUserContext(string sessionToken)
        {
            using (OpenAccessContext context = this.NewContext())
            {
                IUserDto user = this.GetUserInfo(context, sessionToken);
                if (user == null)
                {
                    throw new ArgumentException("Invalid session Token");
                }
                this.SessionManager.UpdateSession(context, sessionToken);
                return new UserContext(user);
            }
        }

        /// <summary>
        /// Gets a contextualized business object.
        /// </summary>
        /// <typeparam name="TDto">The type of the dto.</typeparam>
        /// <param name="sessionToken">The session token.</param>
        /// <returns></returns>
        public ContextBO<TDto> GetBO<TDto>(string sessionToken)
        {
            IUserContext context = this.GetUserContext(sessionToken);
            return context.NewBO<TDto>();
        }

        public object DynamicGetBO(Type dtoType, string sessionToken)
        {
            MethodInfo genericMethod;
            if (boMethods.ContainsKey(dtoType))
            {
                genericMethod = boMethods[dtoType];
            }
            else
            {
                if (this.getBoMethod == null)
                {
                    this.getBoMethod = this.GetType().GetMethod("GetBO");
                }
                genericMethod = this.getBoMethod.MakeGenericMethod(dtoType);
                this.boMethods.Add(dtoType, genericMethod);
            }
            return genericMethod.Invoke(this, new object[] { sessionToken });
        }

        #endregion

        #endregion
    }
}
