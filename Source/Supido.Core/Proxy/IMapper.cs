using System;

namespace Supido.Core.Proxy
{
    /// <summary>
    /// Interface for a property name mapper between two different types
    /// </summary>
    public interface IMapper
    {
        #region - Properties -

        /// <summary>
        /// The source type
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// The target type
        /// </summary>
        Type TargetType { get; }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns></returns>
        string GetSourceName(string targetName);

        /// <summary>
        /// Gets the name of the target.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        string GetTargetName(string sourceName);

        /// <summary>
        /// Gets the source names.
        /// </summary>
        /// <returns></returns>
        string[] GetSourceNames();

        /// <summary>
        /// Gets the target names.
        /// </summary>
        /// <returns></returns>
        string[] GetTargetNames();

        /// <summary>
        /// Adds default mapping.
        /// </summary>
        /// <returns></returns>
        IMapper DefaultMap();

        /// <summary>
        /// Maps bidirectional property mapping
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the target.</param>
        /// <returns></returns>
        IMapper Map(string srcName, string tgtName);

        /// <summary>
        /// Maps unidirectional from source to target.
        /// </summary>
        /// <param name="srcName">Name of the source.</param>
        /// <param name="tgtName">Name of the target.</param>
        /// <returns></returns>
        IMapper MapSrc(string srcName, string tgtName);

        /// <summary>
        /// Maps unidirectional from target to source.
        /// </summary>
        /// <param name="tgtName">Name of the target.</param>
        /// <param name="srcName">Name of the source.</param>
        /// <returns></returns>
        IMapper MapTgt(string tgtName, string srcName);

        /// <summary>
        /// Maps to target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        object MapToTarget(object source);

        /// <summary>
        /// Maps to source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        object MapToSource(object target);

        #endregion
    }
}
