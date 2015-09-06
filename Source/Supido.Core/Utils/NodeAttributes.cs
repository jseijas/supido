using System.Collections.Specialized;
using System.Xml;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Helper for reading attributes from a xml node.
    /// </summary>
    public class NodeAttributes : NameValueCollection
    {
        #region - Fields -

        /// <summary>
        /// The node.
        /// </summary>
        private XmlNode node;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public XmlNode Node
        {
            get { return this.node; }
        }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeAttributes"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public NodeAttributes(XmlNode node)
        {
            this.node = node;
            this.Parse();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Parses the attributes of the node.
        /// </summary>
        protected void Parse()
        {
            this.Clear();
            if (node != null)
            {
                foreach (XmlAttribute attribute in this.node.Attributes)
                {
                    this.Add(attribute.Name, attribute.Value);
                }
            }
        }

        /// <summary>
        /// Gets attribute as string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string AsString(string name, string defaultValue = "")
        {
            string result = this[name];
            return result == null ? defaultValue : result;
        }

        /// <summary>
        /// Gets attribute as byte.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public byte AsByte(string name, byte defaultValue = 0)
        {
            string value = this[name];
            try
            {
                return value == null ? defaultValue : XmlConvert.ToByte(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets attribute as double.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public double AsDouble(string name, double defaultValue = 0)
        {
            string value = this[name];
            try
            {
                return value == null ? defaultValue : XmlConvert.ToDouble(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets attribute as integer.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int AsInteger(string name, int defaultValue = 0)
        {
            string value = this[name];
            try
            {
                return value == null ? defaultValue : XmlConvert.ToInt32(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets attribute as bool.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public bool AsBool(string name, bool defaultValue = false)
        {
            string value = this[name];
            try
            {
                if (value == null)
                {
                    return false;
                }
                return value.Equals("true");
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion
    }
}
