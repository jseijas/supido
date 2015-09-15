using System;
using System.Collections.Generic;
using System.Linq;

namespace Supido.Core.Proxy
{
    /// <summary>
    /// Default Type Mapper
    /// </summary>
    public class TypeMapper : IMapper
    {
        #region - Fields -

        /// <summary>
        /// Property map, source to target
        /// </summary>
        private Dictionary<string, string> srcToTgt = new Dictionary<string, string>();

        /// <summary>
        /// Property map, target to source
        /// </summary>
        private Dictionary<string, string> tgtToSrc = new Dictionary<string, string>();

        #endregion

        #region - Properties -

        /// <summary>
        /// The source type
        /// </summary>
        public Type SourceType { get; private set; }

        /// <summary>
        /// The target type
        /// </summary>
        public Type TargetType { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapper"/> class.
        /// </summary>
        /// <param name="srcType">Type of the source.</param>
        /// <param name="tgtType">Type of the TGT.</param>
        /// <param name="sourceNames">The source names.</param>
        /// <param name="targetNames">The target names.</param>
        public TypeMapper(Type srcType, Type tgtType, string[] sourceNames, string[] targetNames)
        {
            this.SourceType = srcType;
            this.TargetType = tgtType;
            for (int i = 0; i < sourceNames.Length; i++)
            {
                this.AddPropertyMap(sourceNames[i], targetNames[i]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapper"/> class.
        /// </summary>
        /// <param name="srcType">Type of the source.</param>
        /// <param name="tgtType">Type of the TGT.</param>
        public TypeMapper(Type srcType, Type tgtType)
        {
            this.SourceType = srcType;
            this.TargetType = tgtType;
        }

        #endregion

        #region - Methods -

        #region - Private Methods -

        /// <summary>
        /// Adds the source to target.
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the TGT.</param>
        private void AddSourceToTarget(string srcName, string tgtName)
        {
            if (this.srcToTgt.ContainsKey(srcName))
            {
                this.srcToTgt[srcName] = tgtName;
            }
            else
            {
                this.srcToTgt.Add(srcName, tgtName);
            }
        }

        /// <summary>
        /// Adds the target to source.
        /// </summary>
        /// <param name="tgtName">Name of the TGT.</param>
        /// <param name="srcName">Name of the source.</param>
        private void AddTargetToSource(string tgtName, string srcName)
        {
            if (this.tgtToSrc.ContainsKey(tgtName))
            {
                this.tgtToSrc[tgtName] = srcName;
            }
            else
            {
                this.tgtToSrc.Add(tgtName, srcName);
            }
        }

        /// <summary>
        /// Adds the property map.
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the TGT.</param>
        private void AddPropertyMap(string srcName, string tgtName)
        {
            this.AddSourceToTarget(srcName, tgtName);
            this.AddTargetToSource(tgtName, srcName);
        }

        #endregion

        /// <summary>
        /// Adds default mapping.
        /// </summary>
        /// <returns></returns>
        public IMapper DefaultMap()
        {
            IObjectProxy proxySource = ObjectProxyFactory.GetByType(this.SourceType);
            IObjectProxy proxyTarget = ObjectProxyFactory.GetByType(this.TargetType);
            IList<string> sourceNames = proxySource.PropertyNames.ToList();
            IList<string> targetNames = proxyTarget.PropertyNames.ToList();
            foreach (string sourceName in sourceNames)
            {
                if (targetNames.IndexOf(sourceName) > -1)
                {
                    this.AddPropertyMap(sourceName, sourceName);
                }
            }
            return this;
        }

        /// <summary>
        /// Maps bidirectional property mapping
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the target.</param>
        /// <returns></returns>
        public IMapper Map(string srcName, string tgtName)
        {
            this.AddPropertyMap(srcName, tgtName);
            return this;
        }

        /// <summary>
        /// Maps unidirectional from source to target.
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the target.</param>
        /// <returns></returns>
        public IMapper MapSrc(string srcName, string tgtName)
        {
            this.AddSourceToTarget(srcName, tgtName);
            return this;
        }

        /// <summary>
        /// Maps unidirectional from target to source.
        /// </summary>
        /// <param name="tgtName">Name of the target.</param>
        /// <param name="srcName">Name of the source.</param>
        /// <returns></returns>
        public IMapper MapTgt(string tgtName, string srcName)
        {
            this.AddTargetToSource(tgtName, srcName);
            return this;
        }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns></returns>
        public string GetSourceName(string targetName)
        {
            if (this.tgtToSrc.ContainsKey(targetName))
            {
                return this.tgtToSrc[targetName];
            }
            return null;
        }

        /// <summary>
        /// Gets the name of the target.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        public string GetTargetName(string sourceName)
        {
            if (this.srcToTgt.ContainsKey(sourceName))
            {
                return this.srcToTgt[sourceName];
            }
            return null;
        }

        /// <summary>
        /// Gets the source names.
        /// </summary>
        /// <returns></returns>
        public string[] GetSourceNames()
        {
            return this.srcToTgt.Keys.ToArray();
        }

        /// <summary>
        /// Gets the target names.
        /// </summary>
        /// <returns></returns>
        public string[] GetTargetNames()
        {
            return this.tgtToSrc.Keys.ToArray();
        }

        /// <summary>
        /// Maps to target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public object MapToTarget(object source)
        {
            if (source == null)
            {
                return null;
            }
            IObjectProxy proxySource = ObjectProxyFactory.GetByType(this.SourceType);
            IObjectProxy proxyTarget = ObjectProxyFactory.GetByType(this.TargetType);
            object result = proxyTarget.CreateObject();
            foreach (KeyValuePair<string, string> entry in this.srcToTgt)
            {
                proxyTarget.SetValue(result, entry.Value, proxySource.GetValue(source, entry.Key));
            }
            return result;
        }

        /// <summary>
        /// Maps to source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public object MapToSource(object target)
        {
            if (target == null)
            {
                return null;
            }
            IObjectProxy proxySource = ObjectProxyFactory.GetByType(this.SourceType);
            IObjectProxy proxyTarget = ObjectProxyFactory.GetByType(this.TargetType);
            object result = proxySource.CreateObject();
            foreach (KeyValuePair<string, string> entry in this.tgtToSrc)
            {
                proxySource.SetValue(result, entry.Value, proxyTarget.GetValue(target, entry.Key));
            }
            return result;
        }

        #endregion


    }
}
