using Supido.Core.Utils;
using Supido.Templates.Parsing.Evaluators;
using System.IO;

namespace Supido.Templates.Engine
{
    /// <summary>
    /// Class for a Template Action
    /// </summary>
    public class TemplateAction
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent template rule.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TemplateRule Parent { get; private set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; } // CreateFolder, CopyFile, Iterate, CopyFileAvoid, IterateAvoid

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateAction"/> class.
        /// </summary>
        public TemplateAction(TemplateRule parent)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateAction"/> class.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public TemplateAction(TemplateRule parent, string actionType, string source, string target)
            : this(parent)
        {
            this.ActionType = actionType;
            this.Source = source;
            this.Target = target;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Creates a new folder
        /// </summary>
        /// <param name="target">The target.</param>
        private void CreateFolder(string target)
        {
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
        }

        /// <summary>
        /// Deletes a folder.
        /// </summary>
        /// <param name="target">The target.</param>
        private void DeleteFolder(string target)
        {
            if (Directory.Exists(target))
            {
                Directory.Delete(target);
            }
        }

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        private void CopyFile(string source, string target)
        {
            File.Copy(source, target, true);
        }

        /// <summary>
        /// Copies the file avoiding overwrite.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        private void CopyFileAvoid(string source, string target)
        {
            if (!File.Exists(target))
            {
                this.CopyFile(source, target);
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="target">The target.</param>
        private void DeleteFile(string target)
        {
            if (File.Exists(target))
            {
                File.Delete(target);
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private string GetFile(string fileName)
        {
            return this.Parent.Parent.GetFile(fileName);
        }

        /// <summary>
        /// Transforms the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        private void Transform(TemplateContainer container, string source, string target)
        {
            string file = this.GetFile(source);
            if (!string.IsNullOrEmpty(file))
            {
                container.AddAttribute("IteratorCurrentSource", source);
                container.AddAttribute("IteratorCurrentTarget", target);
                string transformed = TemplateTransformer.Transform(file);
                string outputFile = JavascriptEvaluator.ReplaceString(transformed, container);
                StringUtil.WriteFile(target, outputFile);
            }
        }

        /// <summary>
        /// Transforms the avoid.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        private void TransformAvoid(TemplateContainer container, string source, string target)
        {
            if (!File.Exists(target))
            {
                this.Transform(container, source, target);
            }
        }

        /// <summary>
        /// Executes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Execute(TemplateContainer container)
        {
            string source = JavascriptEvaluator.ReplaceString(this.Source, container);
            string target = JavascriptEvaluator.ReplaceString(this.Target, container);
            string actionType = this.ActionType.ToLower();
            if (actionType.Equals("createfolder"))
            {
                this.CreateFolder(target);
            }
            else if (actionType.Equals("deletefolder"))
            {
                this.DeleteFolder(target);
            }
            else if (actionType.Equals("copyfile"))
            {
                this.CopyFile(source, target);
            }
            else if (actionType.Equals("copyfileavoid"))
            {
                this.CopyFileAvoid(source, target);
            }
            else if (actionType.Equals("deletefile"))
            {
                this.DeleteFile(target);
            }
            else if (actionType.Equals("transform"))
            {
                this.Transform(container, source, target);
            }
            else if (actionType.Equals("transformavoid"))
            {
                this.TransformAvoid(container, source, target);
            }
        }

        #endregion
    }
}
