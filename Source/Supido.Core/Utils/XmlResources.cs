using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Xml resources loader and utils
    /// </summary>
    public static class XmlResources
    {
        #region - Static Methods -

        #region - Private Static Methods -

        /// <summary>
        /// Extracts the name of the file system.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        private static string ExtractFileSystemName(string resourceName)
        {
            if (resourceName == null)
            {
                return null;
            }
            int posSeparator = resourceName.IndexOf("://");
            if (posSeparator < 0)
            {
                return resourceName;
            }
            if (resourceName.Length > posSeparator + 3)
            {
                while (resourceName[++posSeparator] == '/')
                {
                }
            }
            return resourceName.Substring(posSeparator);
        }

        #endregion

        /// <summary>
        /// Gets from resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static XmlDocument GetFromResource(string resourceName)
        {
            XmlDocument result = null;
            resourceName = ExtractFileSystemName(resourceName);
            if (!File.Exists(resourceName))
            {
                resourceName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceName);
            }
            if (File.Exists(resourceName))
            {
                result = new XmlDocument();
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(resourceName);
                    result.Load(reader);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Unable to load xml resource file \"{0}\". Cause : {1}", resourceName, ex.Message), ex);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static XmlDocument GetFromUrl(string url)
        {
            XmlDocument result = new XmlDocument();
            try
            {
                result.Load(url);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to load xml from url \"{0}\". Cause : {1}", url, ex.Message), ex);
            }
            return result;
        }

        /// <summary>
        /// Gets from embedded resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static XmlDocument GetFromEmbeddedResource(string resourceName)
        {
            XmlDocument result = new XmlDocument();
            bool loaded = false;
            BaseAssemblyInfo fileInfo = new BaseAssemblyInfo();
            fileInfo.OriginalName = resourceName;
            if (fileInfo.IsAssemblyReady)
            {
                Assembly assembly = Assembly.Load(fileInfo.AssemblyName);
                Stream stream = assembly.GetManifestResourceStream(fileInfo.ResourceFileName);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream(fileInfo.Name);
                }
                if (stream != null)
                {
                    try
                    {
                        result.Load(stream);
                        loaded = true;
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Unable to load xml from embedded resource \"{0}\". Cause : {1}", resourceName, ex.Message), ex);
                    }
                }
            }
            else
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    Stream stream = assembly.GetManifestResourceStream(fileInfo.Name);
                    if (stream != null)
                    {
                        try
                        {
                            result.Load(stream);
                            loaded = true;
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(string.Format("Unable to load xml from embedded resource \"{0}\". Cause : {1}", resourceName, ex.Message), ex);
                        }
                        break;
                    }
                }
            }
            if (!loaded)
            {
                throw new ApplicationException(string.Format("Unable to load xml from embedded resource \"{0}\".", resourceName));
            }
            return result;
        }

        /// <summary>
        /// Gets the embedded resource stream.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(string resourceName)
        {
            Stream stream = null;
            BaseAssemblyInfo fileInfo = new BaseAssemblyInfo();
            fileInfo.OriginalName = resourceName;
            if (fileInfo.IsAssemblyReady)
            {
                Assembly assembly = Assembly.Load(fileInfo.AssemblyName);
                stream = assembly.GetManifestResourceStream(fileInfo.ResourceFileName);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream(fileInfo.Name);
                }
            }
            else
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    if (!(assembly is AssemblyBuilder))
                    {
                        stream = assembly.GetManifestResourceStream(fileInfo.Name);
                        if (stream != null)
                        {
                            break;
                        }
                    }
                }
            }
            return stream;
        }

        #endregion
    }
}
