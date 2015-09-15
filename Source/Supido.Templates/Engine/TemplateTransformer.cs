using System;
using System.Text;

namespace Supido.Templates.Engine
{
    /// <summary>
    /// Given a template, transform it to javascript.
    /// </summary>
    public class TemplateTransformer
    {
        #region - Fields -

        /// <summary>
        /// The current position
        /// </summary>
        protected int currentPosition = 0;

        /// <summary>
        /// The source length
        /// </summary>
        protected int sourceLength = 0;

        /// <summary>
        /// The current character
        /// </summary>
        protected char currentChar = '\0';

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the source value.
        /// </summary>
        /// <value>
        /// The source value.
        /// </value>
        public string SourceValue { get; set; }

        /// <summary>
        /// Gets or sets the open script.
        /// </summary>
        /// <value>
        /// The open script.
        /// </value>
        public string OpenScript { get; set; }

        /// <summary>
        /// Gets or sets the close script.
        /// </summary>
        /// <value>
        /// The close script.
        /// </value>
        public string CloseScript { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateTransformer"/> class.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        public TemplateTransformer(string sourceValue)
        {
            this.OpenScript = "[%";
            this.CloseScript = "%]";
            this.SourceValue = sourceValue;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Gets the next character and move the pointer.
        /// </summary>
        /// <returns></returns>
        private bool GetChar()
        {
            if (this.currentPosition < this.sourceLength)
            {
                this.currentChar = this.SourceValue[this.currentPosition++];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Peek the next character.
        /// </summary>
        /// <returns></returns>
        private char NextChar()
        {
            if (this.currentPosition < this.sourceLength)
            {
                return this.SourceValue[this.currentPosition];
            }
            return '\0';
        }

        /// <summary>
        /// Nexts the character.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private char NextChar(int offset)
        {
            if (this.currentPosition + offset - 1 < this.sourceLength)
            {
                return this.SourceValue[this.currentPosition + offset - 1];
            }
            return '\0';
        }

        /// <summary>
        /// Currents the sequence is.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        private bool CurrentSequenceIs(string s)
        {
            int l = s.Length;
            for (int i = 1; i <= l; i++)
            {
                if (i == 1)
                {
                    if (this.currentChar != s[i - 1])
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.NextChar(i - 1) != s[i - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified s is identifier.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public bool IsIdentifier(string s)
        {
            s = s.ToLower().Trim();
            if (s.Equals("else"))
            {
                return false;
            }
            if (s.Contains("("))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Scans this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">The input has a non closed script</exception>
        public string Scan()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("var result = '';");
            this.currentPosition = 0;
            this.sourceLength = this.SourceValue.Length;
            bool isLiteral = true;
            StringBuilder sb = new StringBuilder();
            while (this.GetChar())
            {
                if (isLiteral)
                {
                    if (this.CurrentSequenceIs(this.OpenScript))
                    {
                        this.GetChar();
                        if (sb.Length > 0)
                        {
                            result.AppendLine("    result = result+'"+sb.ToString()+"';");
                            sb.Clear();
                        }
                        isLiteral = false;
                    }
                    else
                    {
                        sb.Append(this.currentChar);
                    }
                }
                else
                {
                    if (this.CurrentSequenceIs(this.CloseScript))
                    {
                        this.GetChar();
                        string value = sb.ToString().Trim();
                        if (!value.ToLower().Equals("end"))
                        {
                            if (IsIdentifier(value))
                            {
                                result.AppendLine("result = result + " + sb.ToString() + ";");
                            }
                            else
                            {
                                result.AppendLine(sb.ToString());
                                result.AppendLine("{");
                            }
                        }
                        else
                        {
                            result.AppendLine("}");
                        }
                        sb.Clear();
                        isLiteral = true;
                    }
                    else
                    {
                        sb.Append(this.currentChar);
                    }
                }
            }
            if (sb.Length > 0)
            {
                if (isLiteral)
                {
                    result.AppendLine("{");
                    result.AppendLine("    result = result+'" + sb.ToString() + "';");
                    result.AppendLine("}");
                }
                else
                {
                    throw new Exception("The input has a non closed script");
                }
            }
            result.AppendLine("result;");
            return result.ToString();
        }

        /// <summary>
        /// Transforms the specified template to javascript.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string Transform(string s)
        {
            TemplateTransformer tt = new TemplateTransformer(s);
            return tt.Scan();
        }

        #endregion
    }
}
