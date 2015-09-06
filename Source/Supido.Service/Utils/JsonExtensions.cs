﻿using Newtonsoft.Json;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace Supido.Service.Utils
{
    /// <summary>
    /// Extensions for Json
    /// </summary>
    public static class JsonExtensions
    {
        #region - Methods -

        /// <summary>
        /// Converts to json stream message
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Message ToJsonMessage(this object obj)
        {
            return WebOperationContext.Current.CreateTextResponse(JsonConvert.SerializeObject(obj), "application/json; charset=utf-8", Encoding.UTF8);
        }

        /// <summary>
        /// Deserializes from Json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string stream)
        {
            return JsonConvert.DeserializeObject<T>(stream);
        }

        public static object DeserializeJson(this string stream, Type type) 
        {
            return JsonConvert.DeserializeObject(stream, type);

        }

        #endregion
    }
}
