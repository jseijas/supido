using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Structure
{
    public class StructureParser
    {
        #region - Fields -

        private Dictionary<string, DataType> dataTypeMap = new Dictionary<string, DataType>();

        private Dictionary<string, StructureItem> itemMap = new Dictionary<string, StructureItem>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the source text.
        /// </summary>
        /// <value>
        /// The source text.
        /// </value>
        public string SourceText { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureParser"/> class.
        /// </summary>
        public StructureParser()
        {
            this.SourceText = string.Empty;
            this.DefaultTypes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureParser"/> class.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        public StructureParser(string sourceText)
        {
            this.SourceText = sourceText;
            this.DefaultTypes();
        }

        #endregion

        #region - Methods -

        #region - Private Methods -

        private void DefaultTypes()
        {
            this.dataTypeMap.Add("int8", DataType.Tinyint);
            this.dataTypeMap.Add("tinyint", DataType.Tinyint);
            this.dataTypeMap.Add("int16", DataType.Smallint);
            this.dataTypeMap.Add("smallint", DataType.Smallint);
            this.dataTypeMap.Add("int", DataType.Int);
            this.dataTypeMap.Add("int32", DataType.Int);
            this.dataTypeMap.Add("int64", DataType.Bigint);
            this.dataTypeMap.Add("bigint", DataType.Bigint);
            this.dataTypeMap.Add("float", DataType.Float);
            this.dataTypeMap.Add("numeric", DataType.Numeric);
            this.dataTypeMap.Add("date", DataType.Date);
            this.dataTypeMap.Add("datetime", DataType.DateTime);
            this.dataTypeMap.Add("money", DataType.Money);
            this.dataTypeMap.Add("string", DataType.String);
            this.dataTypeMap.Add("bool", DataType.Boolean);
        }


        private string[] PopAliases(ref string s, int lineNumber)
        {
            int i = s.IndexOf("aliases:");
            if (i == -1)
            {
                return null;
            }
            string fromaliases = s.Substring(i, s.Length - i);
            int j = fromaliases.IndexOf(']');
            if (j == -1)
            {
                throw new Exception(string.Format("Aliases defined but no ] found at line {0}", lineNumber));
            }
            s = s.Remove(i, j + 1);
            fromaliases = fromaliases.Remove(j, fromaliases.Length - j);
            i = fromaliases.IndexOf('[');
            fromaliases = fromaliases.Remove(0, i + 1);
            if (string.IsNullOrEmpty(fromaliases))
            {
                return null;
            }
            string[] result = fromaliases.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < result.Length; k++)
            {
                result[k] = result[k].Trim();
            }
            return result;
        }

        private string PopParenthesis(ref string s)
        {
            int i = s.IndexOf('(');
            string result;
            if (i == -1)
            {
                result = s.Trim();
                s = string.Empty;
                return result;
            }
            result = s.Substring(0, i).Trim();
            s = s.Remove(0, i + 1).Trim();
            s = s.Remove(s.Length - 1, 1);
            s = s.Trim();
            return result;
        }

        private StructureDomain ParseDomain(string name, string line, int lineNumber)
        {
            StructureDomain result = new StructureDomain(name);
            this.itemMap.Add(name.ToLower(), result);
            line = StringUtil.RemoveFirstAndLast(line);
            string[] aliases = this.PopAliases(ref line, lineNumber);
            if (aliases != null)
            {
                foreach (string alias in aliases)
                {
                    result.Aliases.Add(alias);
                    this.itemMap.Add(alias.ToLower(), result);
                }
            }
            string[] tokens = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                string[] words = token.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string key = words[0].Trim().ToLower();
                string value = words.Length == 1 ? string.Empty : words[1].Trim();
                if (key.Equals("type"))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new Exception(string.Format("You must define a data type at line {0}", lineNumber));
                    }
                    string typeName = this.PopParenthesis(ref value);
                    result.DataTypeStr = typeName;
                    result.DataType = this.CalculateDataType(typeName, lineNumber);
                    value = value.Trim();
                    result.DataTypeDecimals = 0;
                    result.DataTypeLength = 0;
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] lengths = value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        result.DataTypeLength = Convert.ToInt32(lengths[0].Trim());
                        if (lengths.Length > 1)
                        {
                            result.DataTypeDecimals = Convert.ToInt32(lengths[1].Trim());
                        }
                    }
                }
                else if (key.Equals("identity"))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        result.IsIdentity = true;
                    }
                    else
                    {
                        result.IsIdentity = value.ToLower().Equals("true");
                    }
                }
            }
            return result;
        }

        private DataType CalculateDataType(string typeName, int lineNumber)
        {
            if (this.dataTypeMap.ContainsKey(typeName.ToLower()))
            {
                return this.dataTypeMap[typeName.ToLower()];
            }
            throw new Exception(string.Format("Type not found '{0}' at line {1}", typeName, lineNumber));
        }

        private StructureProperty ParseProperty(string line, int lineNumber)
        {
            StructureProperty result = new StructureProperty();
            string[] tokens = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string name = tokens[0].Trim();
            if (name.EndsWith("?"))
            {
                result.IsNullable = true;
                name = name.Substring(0, name.Length - 1);

            }
            else if (name.EndsWith("+"))
            {
                result.IsPrimaryKey = true;
                name = name.Substring(0, name.Length - 1);
                
            }
            else if (name.EndsWith("*"))
            {
                result.IsPrimaryKey = true;
                result.IsIdentity = true;
                name = name.Substring(0, name.Length - 1);
            }
            result.Name = name;
            if (tokens.Length == 1)
            {
                result.ReferenceName = result.Name;
            }
            else
            {
                string referenceName = tokens[1].Trim();
                if (string.IsNullOrEmpty(referenceName))
                {
                    result.ReferenceName = string.Empty;
                }
                else
                {
                    if (referenceName.StartsWith("$"))
                    {
                        result.ReferenceName = referenceName.Substring(1);
                    }
                    else
                    {
                        result.ReferenceName = referenceName;
                        result.IsTyped = true;
                        string typeName = this.PopParenthesis(ref referenceName);
                        result.DataTypeStr = typeName;
                        result.DataType = this.CalculateDataType(typeName, lineNumber);
                        referenceName = referenceName.Trim();
                        result.DataTypeLength = 0;
                        result.DataTypeDecimals = 0;
                        if (!string.IsNullOrEmpty(referenceName))
                        {
                            string[] lengths = referenceName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                            result.DataTypeLength = Convert.ToInt32(lengths[0].Trim());
                            if (lengths.Length > 1)
                            {
                                result.DataTypeDecimals = Convert.ToInt32(lengths[1].Trim());
                            }
                        }
                    }
                }
            }
            return result;
        }

        private StructureEntity ParseEntity(string name, string line, int lineNumber)
        {
            StructureEntity result = new StructureEntity(name);
            this.itemMap.Add(name.ToLower(), result);
            string[] propertyTokens = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string propertyToken in propertyTokens)
            {
                result.Properties.Add(this.ParseProperty(propertyToken, lineNumber));
            }
            return result;
        }

        private StructureItem ParseLine(string line, int lineNumber)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }
            if (line[0] == '#')
            {
                return null;
            }
            if (line[0] == '-')
            {
                return null;
            }
            string name = StringUtil.PopUntil(ref line, ":");
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception(string.Format("Name not found at line {0}", lineNumber));
            }
            if (string.IsNullOrEmpty(line))
            {
                throw new Exception(string.Format("Information not found at line {0} with name {1}", lineNumber, name));
            }
            if (line[0].Equals('{'))
            {
                return this.ParseDomain(name, line, lineNumber);
            }
            else
            {
                return this.ParseEntity(name, line, lineNumber);
            }
        }

        #endregion

        #region - Public Methods -

        public StructureTree Parse()
        {
            StructureTree result = new StructureTree();
            string[] lines = StringUtil.SplitLines(this.SourceText);
            for (int i = 0; i < lines.Length; i++)
            {
                StructureItem item = this.ParseLine(lines[i], i);
                if (item != null)
                {
                    result.AddItem(item);
                }
            }
            // All lines loaded, structure items mapped, let's resolve references
            foreach (StructureEntity entity in result.Entities)
            {
                foreach (StructureProperty property in entity.Properties)
                {
                    if ((!property.IsTyped) && (!string.IsNullOrEmpty(property.ReferenceName)))
                    {
                        property.ReferenceItem = this.itemMap[property.ReferenceName.ToLower()];
                        if (property.ReferenceItem is StructureDomain)
                        {
                            StructureDomain domain = property.ReferenceItem as StructureDomain;
                            property.DataTypeStr = domain.DataTypeStr;
                            property.DataType = domain.DataType;
                            property.DataTypeLength = domain.DataTypeLength;
                            property.DataTypeDecimals = domain.DataTypeDecimals;
                            if (domain.IsIdentity)
                            {
                                property.IsIdentity = true;
                                property.IsPrimaryKey = true;
                                property.IsNullable = false;
                            }

                        }
                        else
                        {

                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #endregion
    }
}
