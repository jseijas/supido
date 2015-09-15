using Supido.Core.Utils;
using Supido.Templates.Parsing.Grammars;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Class for a json object.
    /// </summary>
    public class JsonObject : DynamicObject, IEnumerable<KeyValuePair<string, object>>
    {
        #region - Fields -

        /// <summary>
        /// The expando object.
        /// </summary>
        private ExpandoObject expando = new ExpandoObject();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets as dictionary.
        /// </summary>
        /// <value>
        /// As dictionary.
        /// </value>
        public IDictionary<string, Object> AsDictionary
        {
            get { return this.expando; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public object this[string name]
        {
            get
            {
                if (this.HasField(name))
                {
                    return this.AsDictionary[StringUtil.Unquote(name)];
                }
                else if (this.HasField("prototype"))
                {
                    return (this.AsDictionary["prototype"] as dynamic)[name];
                }
                else
                {
                    throw new Exception(string.Format("Could not find the name {0}", name));
                }
            }
            set
            {
                this.AsDictionary[StringUtil.Unquote(name)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return this.expando.ElementAt(index).Value;
            }
            set
            {
                this[(this.expando as ICollection<KeyValuePair<string, object>>).ElementAt(index).Key]= value;
            }
        }

        /// <summary>
        /// Gets the key values.
        /// </summary>
        /// <value>
        /// The key values.
        /// </value>
        public IEnumerable<KeyValuePair<string, Object>> KeyValues
        {
            get
            {
                return this.expando;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObject"/> class.
        /// </summary>
        public JsonObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObject"/> class.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        public JsonObject(JsonObject prototype)
        {
            this["prototype"] = prototype;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
        {
            return this.AsDictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.AsDictionary.GetEnumerator();
        }

        /// <summary>
        /// Determines whether this have the specified s field.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public bool HasField(string s)
        {
            return this.AsDictionary.ContainsKey(StringUtil.Unquote(s));
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public JsonObject Clone()
        {
            return new JsonObject(this);
        }

        /// <summary>
        /// Adds an attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string name, dynamic value)
        {
            this[name] = value;
        }

        /// <summary>
        /// Adds the json value to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="sb">The sb.</param>
        private void AddJsonValueToString(object value, StringBuilder sb)
        {
            if (value is JsonObject)
            {
                (value as JsonObject).BuildString(sb);
                return;
            }
            if (value is List<dynamic>)
            {
                List<dynamic> xs = (List<dynamic>)value;
                sb.Append("[");
                for (int i = 0; i < xs.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    this.AddJsonValueToString(xs[i], sb);
                }
                sb.Append("]");
                return;
            }
            sb.Append(value.ToString());
            sb.Append(" ");
        }

        /// <summary>
        /// Builds the string.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <returns></returns>
        protected StringBuilder BuildString(StringBuilder sb)
        {
            sb.AppendLine("{");
            int i = 0;
            foreach (var kv in this.KeyValues)
            {
                if (i++ > 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat("\"{0}\" : ", kv.Key);
                this.AddJsonValueToString(kv.Value, sb);
                sb.AppendLine();
            }
            sb.AppendLine("}");
            return sb;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.BuildString(new StringBuilder()).ToString();
        }

        /// <summary>
        /// Evaluates the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unexpected token type  + token.Name</exception>
        public static dynamic Eval(Token token)
        {
            switch (token.Name)
            {
                case "Name": return Eval(token[0]);
                case "Value": return Eval(token[0]);
                case "Number": return Eval(token[0]);
                case "Integer": return Int32.Parse(token.Text);
                case "Float": return Double.Parse(token.Text);
                case "String": return token.Text.Substring(1, token.Text.Length - 2);
                case "True": return true;
                case "False": return false;
                case "Null": return new JsonObject();
                case "Array": return token.Tokens.Select(Eval).ToList();
                case "Object":
                    {
                        var r = new JsonObject();
                        foreach (Token son in token.Tokens)
                        {
                            var name = son[0].Text;
                            var value = Eval(son[1]);
                            r[name] = value;
                        }
                        return r;
                    }
                default:
                    throw new Exception("Unexpected token type " + token.Name);
            }
        }

        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static JsonObject Parse(string s)
        {
            IList<Token> tokens = JsonGrammar.Object.Parse(s);
            return Eval(tokens[0]);
        }

        #endregion
    }
}
