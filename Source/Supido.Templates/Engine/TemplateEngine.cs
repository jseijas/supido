using Supido.Core.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Supido.Templates.Engine
{
    /// <summary>
    /// Class for a Template Engine
    /// </summary>
    public class TemplateEngine
    {
        #region - Properties -

        /// <summary>
        /// Gets the rules of the template.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        public IList<TemplateRule> Rules { get; private set; }

        private Dictionary<string, string> files = new Dictionary<string, string>();

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngine"/> class.
        /// </summary>
        public TemplateEngine()
        {
            this.Rules = new List<TemplateRule>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Loads the rules.
        /// </summary>
        /// <param name="document">The document.</param>
        private void LoadRules(XmlDocument document, string path)
        {
            foreach (XmlNode node in document.SelectNodes("template/rules/rule"))
            {
                NodeAttributes attributes = new NodeAttributes(node);
                TemplateRule rule = new TemplateRule(this, attributes.AsString("iterator"), attributes.AsString("condition"));
                rule.LoadActions(node, path);
                this.Rules.Add(rule);
            }
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool LoadFromXml(string fileName, string path)
        {
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document == null)
            {
                return false;
            }
            this.LoadRules(document, path);
            return true;
        }

        /// <summary>
        /// Iterates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Iterate(TemplateContainer container)
        {
            foreach (TemplateRule rule in this.Rules)
            {
                rule.Iterate(container);
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public string GetFile(string fileName)
        {
            if (this.files.ContainsKey(fileName))
            {
                return this.files[fileName];
            }
            string result = StringUtil.LoadFile(fileName);
            this.files.Add(fileName, result);
            return result;
        }

        #endregion
    }
}
