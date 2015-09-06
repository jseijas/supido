using Supido.Core.Container;
using Supido.Service.Configuration;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using Supido.Service.Utils;
using System;

namespace Supido.Service.Contracts
{
    /// <summary>
    /// Information contained in a Message
    /// </summary>
    public class MessageInformation
    {
        #region - Properties -

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public Message Message { get; private set; }

        /// <summary>
        /// Gets or sets the absolute URI.
        /// </summary>
        /// <value>
        /// The absolute URI.
        /// </value>
        public string AbsoluteUri { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the path in lower case.
        /// </summary>
        /// <value>
        /// The path in lower case.
        /// </value>
        public string LowPath { get; set; }

        /// <summary>
        /// Gets or sets the path without API.
        /// </summary>
        /// <value>
        /// The path without API.
        /// </value>
        public string PathWithoutApi { get; set; }

        /// <summary>
        /// Gets or sets the path without API in lower case.
        /// </summary>
        /// <value>
        /// The path without API in lower case.
        /// </value>
        public string LowPathWithoutapi { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the path tokens.
        /// </summary>
        /// <value>
        /// The path tokens.
        /// </value>
        public IList<string> PathTokens { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets parameters with lower case keys.
        /// </summary>
        /// <value>
        /// The parameters with lower case keys.
        /// </value>
        public Dictionary<string, string> LowParameters { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInformation"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageInformation(Message message)
        {
            this.Message = message;
            this.Fill();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Fills this instance from the message.
        /// </summary>
        private void Fill()
        {
            this.AbsoluteUri = this.Message.Headers.To.AbsoluteUri;
            this.Host = this.Message.Headers.To.Host;
            this.Port = this.Message.Headers.To.Port;
            this.Path = this.Message.Headers.To.AbsolutePath;
            this.LowPath = this.Path.ToLower();
            IServiceConfiguration configuration = IoC.Get<IServiceConfiguration>();
            if (string.IsNullOrEmpty(configuration.ApiPath))
            {
                this.PathWithoutApi = this.Path;
                this.LowPathWithoutapi = this.LowPath;
            }
            else
            {
                if (this.LowPath.StartsWith("/" + configuration.ApiPath.ToLower() + "/"))
                {
                    this.PathWithoutApi = this.Path.Substring(2 + configuration.ApiPath.Length);
                    this.LowPathWithoutapi = this.PathWithoutApi.ToLower();
                }
                else
                {
                    this.PathWithoutApi = this.Path;
                    this.LowPathWithoutapi = this.LowPath;
                }
            }
            string[] tokens = this.PathWithoutApi.Split('/');
            this.PathTokens = new List<string>();
            foreach (string token in tokens)
            {
                string trimtoken = token.Trim();
                if (!string.IsNullOrEmpty(trimtoken))
                {
                    this.PathTokens.Add(trimtoken);
                }
            }
            this.Query = this.Message.Headers.To.Query;
            this.Parameters = new Dictionary<string, string>();
            this.LowParameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(this.Query))
            {
                string s = this.Query;
                if (s.StartsWith("?"))
                {
                    s = s.Substring(1);
                }
                string[] queryTokens = s.Split('&');
                foreach (string queryToken in queryTokens)
                {
                    int i = queryToken.IndexOf('=');
                    if (i == -1)
                    {
                        this.Parameters.Add(queryToken, "true");
                        this.LowParameters.Add(queryToken.ToLower(), "true");
                    }
                    else
                    {
                        this.Parameters.Add(queryToken.Substring(0, i), queryToken.Substring(i + 1));
                        this.LowParameters.Add(queryToken.Substring(0, i).ToLower(), queryToken.Substring(i + 1));
                    }
                }
            }


            if (this.Message.Properties.ContainsKey("httpRequest"))
            {
                HttpRequestMessageProperty property = (HttpRequestMessageProperty)this.Message.Properties["httpRequest"];
                this.Method = property.Method;
            }
            if (this.Message.IsEmpty)
            {
                this.Body = string.Empty;
            }
            else
            {
                using (var reader = this.Message.GetReaderAtBodyContents())
                {
                    reader.Read();
                    this.Body = System.Text.Encoding.UTF8.GetString(reader.ReadContentAsBase64());
                }
            }
        }

        public T GetBody<T>()
        {
            return this.Body.DeserializeJson<T>();
        }

        public object GetBody(Type type)
        {
            return this.Body.DeserializeJson(type);
        }

        #endregion
    }
}
