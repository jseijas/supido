using System;
using System.Collections.Generic;

namespace Supido.Service.Configuration
{
    /// <summary>
    /// One node on the API tree, representing a path of the rest service.
    /// </summary>
    public class ApiNode
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public ApiNode Parent { get; set; }

        /// <summary>
        /// Gets or sets the type of the dto.
        /// </summary>
        /// <value>
        /// The type of the dto.
        /// </value>
        public Type DtoType { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets the sons.
        /// </summary>
        /// <value>
        /// The sons.
        /// </value>
        public Dictionary<string, ApiNode> Sons { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNode"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public ApiNode(ApiNode parent, string path, Type dtoType, string parameterName)
        {
            this.Parent = parent;
            this.Path = path;
            this.DtoType = dtoType;
            this.ParameterName = parameterName;
            this.Sons = new Dictionary<string, ApiNode>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNode"/> class.
        /// </summary>
        public ApiNode()
        {
            this.Parent = null;
            this.Path = string.Empty;
            this.DtoType = null;
            this.ParameterName = string.Empty;
            this.Sons = new Dictionary<string, ApiNode>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dtoType">Type of the dto.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public ApiNode Add(string path, Type dtoType, string parameterName)
        {
            ApiNode result = new ApiNode(this, path, dtoType, parameterName);
            this.Sons.Add(result.Path.ToLower(), result);
            return result;
        }

        #endregion
    }
}
