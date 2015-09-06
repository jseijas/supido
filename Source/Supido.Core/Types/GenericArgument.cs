using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Supido.Core.Types
{
    /// <summary>
    /// Internal class for parsing generic arguments from assembly names.
    /// </summary>
    internal class GenericArguments
    {
        #region - Constants -

        /// <summary>
        /// The regular expression.
        /// </summary>
        private readonly static Regex regex = new Regex(@"`\d*\[\[", RegexOptions.Compiled);

        #endregion

        #region - Fields -

        /// <summary>
        /// Field for the name of the type
        /// </summary>
        private string typeName = string.Empty;

        /// <summary>
        /// Field for the array of arguments
        /// </summary>
        private string[] arguments = null;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName
        {
            get { return this.typeName; }
        }

        /// <summary>
        /// Gets a value indicating whether [contains arguments].
        /// </summary>
        /// <value><c>true</c> if [contains arguments]; otherwise, <c>false</c>.</value>
        public bool ContainsArguments
        {
            get
            {
                return ((this.arguments != null) && (this.arguments.Length > 0));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is definition.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is definition; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefinition
        {
            get
            {
                if (this.arguments == null)
                {
                    return false;
                }
                foreach (string argument in this.arguments)
                {
                    if (argument.Length > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericArguments"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public GenericArguments(string source)
        {
            this.ParseArguments(source);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Parses the arguments.
        /// </summary>
        /// <param name="source">The source.</param>
        private void ParseArguments(string source)
        {
            if (regex.IsMatch(source))
            {
                this.typeName = source;
            }
            else
            {
                int startIndex = source.IndexOf("[[");
                int endIndex = source.IndexOf("]]");
                if (endIndex >= 0)
                {
                    this.SplitArguments(source.Substring(startIndex + 1, endIndex - startIndex));
                    this.typeName = source.Remove(startIndex, endIndex - startIndex + 2);
                }
            }
        }

        /// <summary>
        /// Splits the arguments.
        /// </summary>
        /// <param name="source">The source.</param>
        private void SplitArguments(string source)
        {
            IList<string> newArguments = new List<string>();
            if (source.Contains("],["))
            {
                newArguments = this.Parse(source);
            }
            else
            {
                string argument = source.Substring(1, source.Length - 2).Trim();
                newArguments.Add(argument);
            }
            this.arguments = new string[newArguments.Count];
            newArguments.CopyTo(this.arguments, 0);
        }

        /// <summary>
        /// Parses the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private IList<string> Parse(string args)
        {
            StringBuilder argument = new StringBuilder();
            IList<string> arguments = new List<string>();
            TextReader input = new StringReader(args);
            try
            {
                int delimiters = 0;
                bool findRight = false;
                do
                {
                    char ch = (char)input.Read();
                    if (ch == '[')
                    {
                        delimiters++;
                        findRight = true;
                    }
                    else if (ch == ']')
                    {
                        delimiters--;
                    }
                    argument.Append(ch);
                    if (findRight && delimiters == 0)
                    {
                        string arg = argument.ToString();
                        arg = arg.Substring(1, arg.Length - 2);
                        arguments.Add(arg);
                        input.Read();
                        argument = new StringBuilder();
                    }
                } while (input.Peek() != -1);
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }
            return arguments;
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <returns></returns>
        public string[] GetArguments()
        {
            if (this.arguments == null)
            {
                return new string[] { };
            }
            return this.arguments;
        }

        #endregion
    }
}
