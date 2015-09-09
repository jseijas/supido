﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Supido.Business;
using Supido.Business.Query;
using Supido.Core.Container;
using Supido.Core.Proxy;
using Supido.Service.Configuration;
using Supido.Service.Contracts;
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
                    JsonSerializer serializer = IoC.Get<JsonSerializer>();
                    // The links getter must be in the MessagePath, so has access to the ApiNode
                    // Method:
                    // 1. Every object/list has access to self
                    // 2. Every object/list has access to parent if parent <> null
                    // 3. If is a list, for every element add a "uri" to the getOne
                    // 4. If is an object, for each son api node add the link of the son. Take care of the byparent!!!!!                   
                    IList<HateoasLink> links = new List<HateoasLink>();
                    links.Add(new HateoasLink("self", information.AbsoluteUri));
                    if (path.HasKeyParameter)
                    {
                        JObject jobject = JObject.FromObject(response, serializer);
                        JArray jarray = JArray.FromObject(links, serializer);
                        if (configuration.IsCamelCase)
                        {
                            jobject.Add("links", jarray);
                        }
                        else
                        {
                            jobject.Add("Links", jarray);
                        }
                        response = jobject;
                    }
                    else
                    {
                        HateoasReponse hateoasResponse = new HateoasReponse();
                        hateoasResponse.Items = response;
                        hateoasResponse.Links = links;
                        JObject jobject = JObject.FromObject(hateoasResponse, serializer);
                        response = jobject;
                    }

                }
                return response.ToJsonMessage(settings, configuration.Indented);
            }
            catch (Exception ex)
            {
                return ex.Message.ToJsonMessage();
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
                    return response.ToJsonMessage();
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
                return response.ToJsonMessage();

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
