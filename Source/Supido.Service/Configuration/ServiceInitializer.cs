using Supido.Business;
using Supido.Business.Meta;
using Supido.Business.Session;
using Supido.Core.Container;
using Supido.Core.Proxy;
using Supido.Core.Types;
using Supido.Core.Utils;
using Supido.Service.Contracts;
using Supido.Service.Cors;
using Supido.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Xml;

namespace Supido.Service.Configuration
{
    public static class ServiceInitializer
    {

        public static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType,
                new[] { propertyType });

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType,
                Type.EmptyTypes);
            ILGenerator getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null,
                new[] { propertyType });
            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
        }

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
            if (entityType == null)
            {
                return null;
            }
            // Let's create a new type? OMG!
            string assname = "Supido.Business.DTO";
            AssemblyName assemblyName = new AssemblyName(assname);

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule("tmpModule");
            TypeBuilder typeBuilder = module.DefineType("BindableRowCellCollection", TypeAttributes.Public | TypeAttributes.Class);
            IObjectProxy entityProxy = ObjectProxyFactory.GetByType(entityType);
            foreach (PropertyInfo propertyInfo in entityProxy.Properties)
            {
                if (TypesManager.IsCommonType(propertyInfo.PropertyType))
                {
                    AddProperty(typeBuilder, propertyInfo.Name, propertyInfo.PropertyType);
                }
            }
            result = typeBuilder.CreateType();
            IoC.Get<ISecurityManager>().Scanner.ProcessDynamicDto(result, entityType);

            
            
            //AppDomain appdomain = AppDomain.CreateDomain("SupidoDynamicTypes", null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase });
            //appdomain.DoCallBack(() =>
            //{
            //    AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assname), AssemblyBuilderAccess.Run);
            //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("SupidoDynamicModule");
            //    TypeBuilder typeBuilder = moduleBuilder.DefineType(entityType.Name + "Dto", TypeAttributes.Public | TypeAttributes.Serializable | TypeAttributes.Class);
            //    IObjectProxy entityProxy = ObjectProxyFactory.GetByType(entityType);
            //    foreach (PropertyInfo propertyInfo in entityProxy.Properties)
            //    {
            //        AddProperty(typeBuilder, propertyInfo.Name, propertyInfo.PropertyType);
            //    }
            //    result = typeBuilder.CreateType();
            //    IoC.Get<ISecurityManager>().Scanner.ProcessDynamicDto(result, entityType);
            //});
            return result;
        }



        private static void ConfigureApi(XmlNode node, ApiNode apinode)
        {
            NodeAttributes attributes = new NodeAttributes(node);
            Type dtoType = GetDtoType(attributes.AsString("dtoType"), attributes.AsString("dtoName"), attributes.AsString("path"), attributes.AsString("entityName"), attributes.AsString("entityType"));
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
            Type dtoType = GetDtoType(attributes.AsString("dtoType"), attributes.AsString("dtoName"), attributes.AsString("path"), attributes.AsString("entityName"), attributes.AsString("entityType"));
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
                    string mappersName = securityAttributes.AsString("mapper");
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
