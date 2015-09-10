using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Supido.Business;
using Supido.Business.Query;
using Supido.Core.Container;
using Supido.Core.Proxy;
using Supido.Service.Configuration;
using Supido.Service.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace Supido.Service.Contracts
{
    /// <summary>
    /// Main class for the Rest Service
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestService : IRestService
    {
        /// <summary>
        /// Options Verb, for CORS
        /// </summary>
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        public void GetOptions()
        {
        }

        private string GetSessionToken(MessageInformation information)
        {
            if (!information.LowParameters.ContainsKey("sessiontoken"))
            {
                throw new ArgumentNullException("A session token must be provided");
            }
            return information.LowParameters["sessiontoken"];
        }

        private object ConvertToHateoas(object source, IServiceConfiguration configuration, MessagePath path, bool isPostPut = false)
        {
            IList<HateoasLink> links = path.Links;
            JsonSerializer serializer = IoC.Get<JsonSerializer>();
            if ((path.HasKeyParameter) || (path.IsByParent) || isPostPut)
            {
                JObject jobject = JObject.FromObject(source, serializer);
                JArray jarray = JArray.FromObject(links, serializer);
                if (configuration.IsCamelCase)
                {
                    jobject.Add("links", jarray);
                }
                else
                {
                    jobject.Add("Links", jarray);
                }
                return jobject;
            }
            else
            {
                HateoasReponse response = new HateoasReponse();
                response.Items = source;
                response.Links = links;
                return JObject.FromObject(response, serializer);
            }
        }

        /// <summary>
        /// Get Verb.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [WebInvoke(Method="GET", UriTemplate = "*", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Message Get(Message message)
        {
            MessageInformation information = new MessageInformation(message);
            try
            {
                string sessionToken = this.GetSessionToken(information);
                MessagePath path = new MessagePath(information);
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                object response;
                if (path.IsByParent)
                {
                    MessagePath parentPath = new MessagePath(information, true);
                    if (!parentPath.HasKeyParameter)
                    {
                        throw new ArgumentException("A link to a parent must have parameter key");
                    }
                    dynamic bo = securityManager.DynamicGetBO(parentPath.DtoType, sessionToken);
                    object parent = bo.GetOne(string.Empty, parentPath.QueryInfo);
                    IObjectProxy proxy = ObjectProxyFactory.Get(parent);
                    object value = proxy.GetValue(parent, path.ParentKeyParameter);
                    QueryInfo query = new QueryInfo();
                    query.Equal(path.KeyParameterName, value.ToString());
                    bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                    response = bo.GetOne(string.Empty, query);
                }
                else
                {
                    dynamic bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                    if (path.HasKeyParameter)
                    {
                        response = bo.GetOne(string.Empty, path.QueryInfo);
                    }
                    else
                    {
                        response = bo.GetAll(path.QueryInfo);
                    }
                }
                IServiceConfiguration configuration = IoC.Get<IServiceConfiguration>();
                JsonSerializerSettings settings = IoC.Get<JsonSerializerSettings>();
                if (configuration.IsHateoas)
                {
                    response = this.ConvertToHateoas(response, configuration, path);
                }
                return response.ToJsonMessage(settings, configuration.Indented);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    return ex.InnerException.Message.ToJsonMessage();
                }
                else
                {
                    return ex.Message.ToJsonMessage();
                }
            }
        }

        /// <summary>
        /// Post Verb
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate="*", RequestFormat = WebMessageFormat.Json, ResponseFormat= WebMessageFormat.Json)]
        public Message Post(Message message)
        {
            MessageInformation information = new MessageInformation(message);
            try
            {
                string sessionToken = this.GetSessionToken(information);
                MessagePath path = new MessagePath(information);
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                IServiceConfiguration configuration = IoC.Get<IServiceConfiguration>();
                JsonSerializerSettings settings = IoC.Get<JsonSerializerSettings>();
                if (path.IsQuery)
                {
                    dynamic bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                    QueryInfo info = information.GetBody<QueryInfo>();
                    info.AddFacetsFrom(path.QueryInfo);
                    object response;
                    if (path.HasKeyParameter)
                    {
                        response = bo.GetOne(string.Empty, info);
                    }
                    else
                    {
                        response = bo.GetAll(info);
                    }
                    if (configuration.IsHateoas)
                    {
                        response = this.ConvertToHateoas(response, configuration, path);
                    }
                    return response.ToJsonMessage(settings, configuration.Indented);
                }
                else 
                { 
                    if (path.HasKeyParameter)
                    {
                        throw new ArgumentException("POST operation cannot have a key parameter in the path.");
                    }
                    dynamic bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                    dynamic dto = information.GetBody(path.DtoType);
                    object response = bo.Insert(dto);
                    if (configuration.IsHateoas)
                    {
                    }
                    return response.ToJsonMessage();
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToJsonMessage();
            }
        }

        /// <summary>
        /// Put Verb
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "*", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Message Put(Message message)
        {
            MessageInformation information = new MessageInformation(message);
            try
            {
                string sessionToken = this.GetSessionToken(information);
                MessagePath path = new MessagePath(information);
                if (path.HasKeyParameter)
                {
                    throw new ArgumentException("POST operation cannot have a key parameter in the path.");
                }
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                dynamic bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                dynamic dto = information.GetBody(path.DtoType);
                object response = bo.Update(dto);
                IServiceConfiguration configuration = IoC.Get<IServiceConfiguration>();
                JsonSerializerSettings settings = IoC.Get<JsonSerializerSettings>();
                if (configuration.IsHateoas)
                {
                    response = this.ConvertToHateoas(response, configuration, path);
                }
                return response.ToJsonMessage(settings, configuration.Indented);
            }
            catch (Exception ex)
            {
                return ex.Message.ToJsonMessage();
            }
        }

        /// <summary>
        /// Delete Verb.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "*", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Message Delete(Message message)
        {
            MessageInformation information = new MessageInformation(message);
            try
            {
                string sessionToken = this.GetSessionToken(information);
                MessagePath path = new MessagePath(information);
                if (!path.HasKeyParameter)
                {
                    throw new ArgumentException("DELETE operation must have a key parameter in the path.");
                }
                ISecurityManager securityManager = IoC.Get<ISecurityManager>();
                dynamic bo = securityManager.DynamicGetBO(path.DtoType, sessionToken);
                object response = bo.Delete(path.KeyParameter);
                return response.ToJsonMessage();

            }
            catch (Exception ex)
            {
                return ex.Message.ToJsonMessage();
            }
        }
    }
}
