
namespace Supido.Core.Utils
{
    /// <summary>
    /// Class for an assembly info (assembly name and inner name).
    /// </summary>
    public class BaseAssemblyInfo
    {
        #region - Constants -

        /// <summary>
        /// Constant for the starter of a nullable type.
        /// </summary>
        public static readonly string NullableTypeStarter = "System.Nullable";

        #endregion

        #region - Fields -

        /// <summary>
        /// Inner name. For example in a file, will be the file name.
        /// </summary>
        protected string name = string.Empty;

        /// <summary>
        /// Name of the assembly.
        /// </summary>
        private string assemblyName = string.Empty;

        /// <summary>
        /// Original name.
        /// </summary>
        private string originalName = string.Empty;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the original name.
        /// </summary>
        /// <value>The original name.</value>
        public string OriginalName
        {
            get
            {
                return this.originalName;
            }
            set
            {
                this.originalName = value;
                this.SplitName();
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public string AssemblyName
        {
            get
            {
                return this.assemblyName;
            }
            set
            {
                this.assemblyName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is assembly ready.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is assembly ready; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssemblyReady
        {
            get
            {
                return ((this.assemblyName != null) && (this.assemblyName.Length > 0));
            }
        }

        /// <summary>
        /// Gets the name of the resource file.
        /// </summary>
        /// <value>The name of the resource file.</value>
        public string ResourceFileName
        {
            get
            {
                return string.IsNullOrEmpty(this.assemblyName) ? this.Name : this.AssemblyName + "." + this.Name;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is type assembly.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is type assembly; otherwise, <c>false</c>.
        /// </value>
        public bool IsTypeAssembly { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAssemblyInfo"/> class.
        /// </summary>
        /// <param name="isTypeAssembly">if set to <c>true</c> [is type assembly].</param>
        public BaseAssemblyInfo(bool isTypeAssembly = false)
        {
            this.IsTypeAssembly = isTypeAssembly;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Splits the name.
        /// </summary>
        protected virtual void SplitName()
        {

            if ((this.IsTypeAssembly) && (this.originalName.StartsWith(NullableTypeStarter)))
            {
                int i = this.originalName.IndexOf("]],");
                if (i < 0)
                {
                    this.name = this.originalName;
                    this.assemblyName = null;
                }
                else
                {
                    this.name = this.originalName.Substring(0, i + 2).Trim();
                    this.assemblyName = this.originalName.Substring(i + 3).Trim();
                }
            }
            else
            {
                int posSeparator = this.originalName.IndexOf(",");
                if (posSeparator < 0)
                {
                    this.name = this.originalName.Trim();
                    this.assemblyName = null;
                }
                else
                {
                    this.name = this.originalName.Substring(0, posSeparator).Trim();
                    this.assemblyName = this.originalName.Substring(posSeparator + 1).Trim();
                }
            }
        }

        #endregion
    }
}
