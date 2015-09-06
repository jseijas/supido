using Supido.Business;
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
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Xml;

namespace Supido.Service.Configuration
{
    public static class ServiceInitializer
    {

        private static void ConfigureApi(XmlNode node, ApiNode apinode)
        {
            NodeAttributes attributes = new NodeAttributes(node);
            Type dtoType = null;
            string typeName = attributes.AsString("dtoType");
            if (string.IsNullOrEmpty(typeName))
            {
                string dtoName = attributes.AsString("dtoName");
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                if (string.IsNullOrEmpty(dtoName))
                {
                    IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByName(attributes.AsString("path"));
                    if (metamodelEntity != null)
                    {
                        dtoType = metamodelEntity.DtoTypes[0];
                    }
                }
                else
                {
                    IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByDtoName(dtoName);
                    if (metamodelEntity != null)
                    {
                        foreach (Type type in metamodelEntity.DtoTypes)
                        {
                            if (dtoName.ToLower().Equals(type.Name.ToLower()))
                            {
                                dtoType = type;
                            }
                        }
                    }
                }
            }
            else
            {
                dtoType = TypesManager.ResolveType(attributes.AsString("dto"));
            }
            if (dtoType != null)
            {
                ApiNode childnode = apinode.AddNode(attributes.AsString("path"), dtoType, attributes.AsString("parameterName"));
                foreach (XmlNode subnode in node.SelectNodes("api"))
                {
                    ConfigureApi(subnode, childnode);
                }
            }
        }

        private static void ConfigureApi(XmlNode node, IServiceConfiguration configuration)
        {
            NodeAttributes attributes = new NodeAttributes(node);
            Type dtoType = null;
            string typeName = attributes.AsString("dtoType");
            if (string.IsNullOrEmpty(typeName))
            {
                string dtoName = attributes.AsString("dtoName");
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                if (string.IsNullOrEmpty(dtoName))
                {
                    IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByName(attributes.AsString("path"));
                    if (metamodelEntity != null)
                    {
                        dtoType = metamodelEntity.DtoTypes[0];
                    }
                }
                else
                {
                    IMetamodelEntity metamodelEntity = securityManager.MetamodelManager.GetEntityByDtoName(dtoName);
                    if (metamodelEntity != null)
                    {
                        foreach (Type type in metamodelEntity.DtoTypes)
                        {
                            if (dtoName.ToLower().Equals(type.Name.ToLower()))
                            {
                                dtoType = type;
                            }
                        }
                    }
                }
            }
            else
            {
                dtoType = TypesManager.ResolveType(attributes.AsString("dto"));
            }
            if (dtoType != null)
            {
                ApiNode childnode = configuration.AddNode(attributes.AsString("path"), dtoType, attributes.AsString("parameterName"));
                foreach (XmlNode subnode in node.SelectNodes("api"))
                {
                    ConfigureApi(subnode, childnode);
                }
            }
        }

        private static void ConfigureSecurityManager(XmlDocument document)
        {
            ISecurityManager securityManager = null;
            XmlNode securityNode = document.SelectSingleNode("configuration/security");
            if (securityNode != null)
            {
                NodeAttributes securityAttributes = new NodeAttributes(securityNode);
                Type sessionManagerType = TypesManager.ResolveType(securityAttributes.AsString("sessionManager"));
                if (sessionManagerType != null)
                {
                    ISessionManager sessionManager = (ISessionManager)Activator.CreateInstance(sessionManagerType);
                    IoC.Register<ISessionManager>(sessionManager);
                }
                Type securityManagerType = TypesManager.ResolveType(securityAttributes.AsString("securityManager"));
                if (securityManagerType != null)
                {
                    string mappersName = securityAttributes.AsString("mapper", "Mappers.xml");
                    securityManager = (ISecurityManager)Activator.CreateInstance(securityManagerType, mappersName);
                    IoC.Register<ISecurityManager>(securityManager);
                }
            }
        }

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
                foreach (XmlNode apiNode in serviceNode.SelectNodes("api"))
                {
                    ConfigureApi(apiNode, configuration);
                }
                IoC.Register<IServiceConfiguration>(configuration);
            }
        }

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
    }
}
