using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Supido.Business;
using Supido.Business.Audit;
using Supido.Business.Meta;
using Supido.Business.Session;
using Supido.Core.Container;
using Supido.Core.Types;
using Supido.Core.Utils;
using Supido.Service.Contracts;
using Supido.Service.Cors;
using Supido.Service.Utils;
using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Web.Routing;
using System.Xml;

namespace Supido.Service.Configuration
{
    /// <summary>
    /// Class for the service initializer.
    /// </summary>
    public static class ServiceInitializer
    {
        #region - Methods -

        #region - Private Static Methods -

        /// <summary>
        /// Try to get the DTO type from the parameters. If the DTO is not found, but an entity type is found, then clone the entity type to create a new DTO type..
        /// </summary>
        /// <param name="dtoTypeName">Name of the dto type.</param>
        /// <param name="dtoName">Name of the dto.</param>
        /// <param name="path">The path.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns></returns>
        private static Type GetDtoType(string dtoTypeName, string dtoName, string path, string entityName, string entityTypeName)
        {
            Type result = null;
            Type entityType = null;
            // Search by type name
            if (!string.IsNullOrEmpty(dtoTypeName))
            {
                result = TypesManager.ResolveType(dtoTypeName);
                if (result != null)
                {
                    return result;
                }
            }
            ISecurityManager securityManager = IoC.Get<ISecurityManager>();
            // Search by dto name
            if (!string.IsNullOrEmpty(dtoTypeName))
            {
                IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByDtoName(dtoName);
                if (metamodelEntity != null)
                {
                    foreach (Type type in metamodelEntity.DtoTypes)
                    {
                        if (dtoName.ToLower().Equals(type.Name.ToLower()))
                        {
                            return type;
                        }
                    }
                }
            }
            // search by entity name
            if (!string.IsNullOrEmpty(entityName))
            {
                IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByName(entityName);
                if (metamodelEntity != null)
                {
                    if (metamodelEntity.DtoTypes.Count > 0)
                    {
                        return metamodelEntity.DtoTypes[0];
                    }
                    else
                    {
                        entityType = metamodelEntity.EntityType;
                    }
                }
            }
            // search by entity type
            if ((!string.IsNullOrEmpty(entityTypeName)) && (entityType == null))
            {
                entityType = TypesManager.ResolveType(entityTypeName);
                if (entityType != null)
                {
                    IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntity(entityType);
                    if ((metamodelEntity != null) && (metamodelEntity.DtoTypes.Count > 0))
                    {
                        return metamodelEntity.DtoTypes[0];
                    }

                }
            }
            // If not found in own metamodel, because is not loaded into the system... but we can get the entity type by name from the telerik metamodel?
            if ((entityType == null) && (!string.IsNullOrEmpty(entityName)))
            {
                entityType = securityManager.Scanner.FindEntityTypeInMetamodel(entityName);
            }
            if ((entityType == null) && (!string.IsNullOrEmpty(path)))
            {
                entityType = securityManager.Scanner.FindEntityTypeInMetamodel(path);
            }
            if (entityType == null)
            {
                return null;
            }
            result = TypeBuilderHelper.CloneCommonType(entityType, AppDomain.CurrentDomain, "Supido.Business.Dto", "SupidoDynamicModule", entityType.Name + "Dto");
            securityManager.Scanner.ProcessDynamicDto(result, entityType);
            return result;
        }

        /// <summary>
        /// Given a type of a DTO and the service path, gets the parameter name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <returns></returns>
        private static string GetParameterName(string path, Type dtoType)
        {
            ISecurityManager securityManager = IoC.Get<ISecurityManager>();
            IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByDto(dtoType);
            if (metamodelEntity != null)
            {
                IList<IMetamodelField> fields = metamodelEntity.GetPkFields();
                if (fields.Count == 1)
                {
                    return fields[0].Name;
                }
            }
            return path + "Pk";
        }

        /// <summary>
        /// Configures the API from a child node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="apinode">The apinode.</param>
        private static void ConfigureApi(XmlNode node, ApiNode apinode)
        {
            NodeAttributes attributes = new NodeAttributes(node);
            Type dtoType = GetDtoType(attributes.AsString("dtoType"), attributes.AsString("dtoName"), attributes.AsString("path"), attributes.AsString("entityName"), attributes.AsString("entityType"));
            if (dtoType != null)
            {
                string parameterName = attributes.AsString("parameterName");
                if (string.IsNullOrEmpty(parameterName))
                {
                    parameterName = GetParameterName(attributes.AsString("path"), dtoType);
                }
                ApiNode childnode = apinode.AddNode(attributes.AsString("path"), dtoType, parameterName, attributes.AsString("byparent"));
                foreach (XmlNode subnode in node.SelectNodes("api"))
                {
                    ConfigureApi(subnode, childnode);
                }
            }
        }

        /// <summary>
        /// Configures the API from a root node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="configuration">The configuration.</param>
        private static void ConfigureApi(XmlNode node, IServiceConfiguration configuration)
        {
            NodeAttributes attributes = new NodeAttributes(node);
            Type dtoType = GetDtoType(attributes.AsString("dtoType"), attributes.AsString("dtoName"), attributes.AsString("path"), attributes.AsString("entityName"), attributes.AsString("entityType"));
            if (dtoType != null)
            {
                string parameterName = attributes.AsString("parameterName");
                if (string.IsNullOrEmpty(parameterName))
                {
                    parameterName = GetParameterName(attributes.AsString("path"), dtoType);
                }
                ApiNode childnode = configuration.AddNode(attributes.AsString("path"), dtoType, parameterName, attributes.AsString("byparent"));
                foreach (XmlNode subnode in node.SelectNodes("api"))
                {
                    ConfigureApi(subnode, childnode);
                }
            }
        }

        /// <summary>
        /// Configures the security manager.
        /// </summary>
        /// <param name="document">The document.</param>
        private static void ConfigureSecurityManager(XmlDocument document)
        {
            ISecurityManager securityManager = null;
            XmlNode securityNode = document.SelectSingleNode("configuration/security");
            if (securityNode != null)
            {
                NodeAttributes securityAttributes = new NodeAttributes(securityNode);
                if (!string.IsNullOrEmpty(securityAttributes.AsString("sessionManager")))
                {
                    Type sessionManagerType = TypesManager.ResolveType(securityAttributes.AsString("sessionManager"));
                    if (sessionManagerType != null)
                    {
                        ISessionManager sessionManager = (ISessionManager)Activator.CreateInstance(sessionManagerType);
                        IoC.Register<ISessionManager>(sessionManager);
                    }
                }
                if (!string.IsNullOrEmpty(securityAttributes.AsString("auditManager")))
                {
                    Type auditManagerType = TypesManager.ResolveType(securityAttributes.AsString("auditManager"));
                    if (auditManagerType != null)
                    {
                        IAuditManager auditManager = (IAuditManager)Activator.CreateInstance(auditManagerType);
                        IoC.Register<IAuditManager>(auditManager);
                    }
                }
                if (!string.IsNullOrEmpty(securityAttributes.AsString("securityManager")))
                {
                    Type securityManagerType = TypesManager.ResolveType(securityAttributes.AsString("securityManager"));
                    if (securityManagerType != null)
                    {
                        securityManager = (ISecurityManager)Activator.CreateInstance(securityManagerType);
                        IoC.Register<ISecurityManager>(securityManager);
                        string mappersName = securityAttributes.AsString("mapper");
                        securityManager.Configure(mappersName);
                    }
                }
            }
        }

        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="document">The document.</param>
        private static void ConfigureService(XmlDocument document)
        {
            ISecurityManager securityManager = IoC.Get<ISecurityManager>();
            XmlNode serviceNode = document.SelectSingleNode("configuration/service");
            if (serviceNode != null)
            {
                NodeAttributes serviceAttributes = new NodeAttributes(serviceNode);
                ServiceConfiguration configuration = new ServiceConfiguration();
                configuration.ApiPath = serviceAttributes.AsString("apiPath");
                configuration.IsCors = serviceAttributes.AsBool("cors");
                configuration.IsHateoas = serviceAttributes.AsBool("hateoas");
                configuration.IsCamelCase = serviceAttributes.AsBool("camel", true);
                configuration.IncludeNulls = serviceAttributes.AsBool("includeNulls", false);
                configuration.Indented = serviceAttributes.AsBool("indented", true);

                JsonSerializer serializer = new JsonSerializer();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                if (configuration.IsCamelCase)
                {
                    serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
                if (configuration.IncludeNulls)
                {
                    serializer.NullValueHandling = NullValueHandling.Include;
                    settings.NullValueHandling = NullValueHandling.Include;
                }
                else
                {
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                }
                IoC.Register(serializer);
                IoC.Register(settings);

                foreach (XmlNode apiNode in serviceNode.SelectNodes("api"))
                {
                    ConfigureApi(apiNode, configuration);
                }
                IoC.Register<IServiceConfiguration>(configuration);
                IoC.Get<IAuditManager>().Configure();
            }
        }

        /// <summary>
        /// Discovers the services.
        /// </summary>
        private static void DiscoverServices()
        {
            IServiceConfiguration configuration = IoC.Get<IServiceConfiguration>();
            ServiceDiscover.DiscoverServices(configuration.ApiPath, AppDomain.CurrentDomain, configuration.IsCors);
            foreach (KeyValuePair<string, ApiNode> kvp in configuration.Root.Sons)
            {
                string name = string.IsNullOrEmpty(configuration.ApiPath) ? kvp.Value.Path.ToLower() : configuration.ApiPath + "/" + kvp.Value.Path.ToLower();
                if (configuration.IsCors)
                {
                    RouteTable.Routes.Add(new ServiceRoute(name, new CorsEnabledServiceHostFactory(), typeof(RestService)));
                }
                else
                {
                    RouteTable.Routes.Add(new ServiceRoute(name, new WebServiceHostFactory(), typeof(RestService)));
                }
            }
        }

        #endregion

        #region - Public Static Methods -

        /// <summary>
        /// Run the scanning for services, initializing from an xml with the information of the service API.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void Initialize(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Supido.xml";
            }
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document == null)
            {
                document = XmlResources.GetFromEmbeddedResource(fileName);
            }
            if (document != null)
            {
                ConfigureSecurityManager(document);
                ConfigureService(document);
            }
            DiscoverServices();
        }

        #endregion

        #endregion
    }
}
